using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Ash.VisibilitySystem
{
    public class SpatialHash
	{
		public Rectangle GridBounds = new Rectangle();

		/// <summary>
		/// the size of each cell in the hash
		/// </summary>
		int _cellSize;

		/// <summary>
		/// 1 over the cell size. cached result due to it being used a lot.
		/// </summary>
		float _inverseCellSize;

		/// <summary>
		/// the Dictionary that holds all of the data
		/// </summary>
		IntIntDictionary _cellDict = new IntIntDictionary();

		/// <summary>
		/// shared HashSet used to return collision info
		/// </summary>
		HashSet<AABB> _tempHashset = new HashSet<AABB>();


		public SpatialHash(int cellSize = 100)
		{
			_cellSize = cellSize;
			_inverseCellSize = 1f / _cellSize;
		}


		/// <summary>
		/// gets the cell x,y values for a world-space x,y value
		/// </summary>
		/// <returns>The coords.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Point CellCoords(int x, int y)
		{
			return new Point(Mathf.FloorToInt(x * _inverseCellSize), Mathf.FloorToInt(y * _inverseCellSize));
		}


		/// <summary>
		/// gets the cell x,y values for a world-space x,y value
		/// </summary>
		/// <returns>The coords.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Point CellCoords(float x, float y)
		{
			return new Point(Mathf.FloorToInt(x * _inverseCellSize), Mathf.FloorToInt(y * _inverseCellSize));
		}


		/// <summary>
		/// gets the cell at the world-space x,y value. If the cell is empty and createCellIfEmpty is true a new cell will be created.
		/// </summary>
		/// <returns>The at position.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="createCellIfEmpty">If set to <c>true</c> create cell if empty.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		List<AABB> CellAtPosition(int x, int y, bool createCellIfEmpty = false)
		{
			List<AABB> cell = null;
			if (!_cellDict.TryGetValue(x, y, out cell))
			{
				if (createCellIfEmpty)
				{
					cell = new List<AABB>();
					_cellDict.Add(x, y, cell);
				}
			}

			return cell;
		}


		/// <summary>
		/// adds the object to the SpatialHash
		/// </summary>
		/// <param name="collider">Object.</param>
		public void Register(AABB collider)
		{
			var bounds = collider.Bounds;
			var p1 = CellCoords(bounds.X, bounds.Y);
			var p2 = CellCoords(bounds.Right, bounds.Bottom);

			// update our bounds to keep track of our grid size
			if (!GridBounds.Contains(p1))
				RectangleExt.Union(ref GridBounds, ref p1, out GridBounds);

			if (!GridBounds.Contains(p2))
				RectangleExt.Union(ref GridBounds, ref p2, out GridBounds);

			for (var x = p1.X; x <= p2.X; x++)
			{
				for (var y = p1.Y; y <= p2.Y; y++)
				{
					// we need to create the cell if there is none
					var c = CellAtPosition(x, y, true);
					c.Add(collider);
				}
			}
		}


		/// <summary>
		/// removes the object from the SpatialHash
		/// </summary>
		/// <param name="collider">Collider.</param>
		public void Remove(AABB collider)
		{
			var bounds = collider.Bounds;
			var p1 = CellCoords(bounds.X, bounds.Y);
			var p2 = CellCoords(bounds.Right, bounds.Bottom);

			for (var x = p1.X; x <= p2.X; x++)
			{
				for (var y = p1.Y; y <= p2.Y; y++)
				{
					// the cell should always exist since this collider should be in all queryed cells
					var cell = CellAtPosition(x, y);
					Insist.IsNotNull(cell, "removing Collider [{0}] from a cell that it is not present in", collider);
					if (cell != null)
						cell.Remove(collider);
				}
			}
		}


		/// <summary>
		/// removes the object from the SpatialHash using a brute force approach
		/// </summary>
		/// <param name="obj">Object.</param>
		public void RemoveWithBruteForce(AABB obj)
		{
			_cellDict.Remove(obj);
		}


		public void Clear()
		{
			_cellDict.Clear();
		}

		/// <summary>
		/// returns all the Colliders in the SpatialHash
		/// </summary>
		/// <returns>The all objects.</returns>
		public HashSet<AABB> GetAllObjects()
		{
			return _cellDict.GetAllObjects();
		}


		#region hash queries

		/// <summary>
		/// returns all objects in cells that the bounding box intersects
		/// </summary>
		/// <returns>The neighbors.</returns>
		/// <param name="bounds">Bounds.</param>
		/// <param name="layerMask">Layer mask.</param>
		public HashSet<AABB> AabbBroadphase(ref RectangleF bounds)
		{
			_tempHashset.Clear();

			var p1 = CellCoords(bounds.X, bounds.Y);
			var p2 = CellCoords(bounds.Right, bounds.Bottom);

			for (var x = p1.X; x <= p2.X; x++)
			{
				for (var y = p1.Y; y <= p2.Y; y++)
				{
					var cell = CellAtPosition(x, y);
					if (cell == null)
						continue;

					// we have a cell. loop through and fetch all the Colliders
					for (var i = 0; i < cell.Count; i++)
					{
						var collider = cell[i];
						if (!collider.Hidden)
							continue;
						if (bounds.Intersects(collider.Bounds))
							_tempHashset.Add(collider);
					}
				}
			}

			return _tempHashset;
		}
		#endregion
	}


	/// <summary>
	/// wraps a Unit32,List<Collider> Dictionary. It's main purpose is to hash the int,int x,y coordinates into a single
	/// Uint32 key which hashes perfectly resulting in an O(1) lookup.
	/// </summary>
	class IntIntDictionary
	{
		Dictionary<long, List<AABB>> _store = new Dictionary<long, List<AABB>>();


		/// <summary>
		/// computes and returns a hash key based on the x and y value. basically just packs the 2 ints into a long.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		long GetKey(int x, int y)
		{
			return unchecked((long)x << 32 | (uint)y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(int x, int y, List<AABB> list)
		{
			_store.Add(GetKey(x, y), list);
		}


		/// <summary>
		/// removes the collider from the Lists the Dictionary stores using a brute force approach
		/// </summary>
		/// <param name="obj">Object.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(AABB obj)
		{
			foreach (var list in _store.Values)
			{
				if (list.Contains(obj))
					list.Remove(obj);
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryGetValue(int x, int y, out List<AABB> list)
		{
			return _store.TryGetValue(GetKey(x, y), out list);
		}


		/// <summary>
		/// gets all the Colliders currently in the dictionary
		/// </summary>
		/// <returns>The all objects.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public HashSet<AABB> GetAllObjects()
		{
			var set = new HashSet<AABB>();

			foreach (var list in _store.Values)
				set.UnionWith(list);

			return set;
		}


		/// <summary>
		/// clears the backing dictionary
		/// </summary>
		public void Clear()
		{
			_store.Clear();
		}
	}
}