using Microsoft.Xna.Framework;


namespace Ash
{
    public class Camera
	{
		/// <summary>
		/// shortcut to entity.transform.position
		/// </summary>
		/// <value>The position.</value>
		public Vector2 Position
		{
			get => _position;
			set { _position = value; _areMatrixesDirty = true; _areBoundsDirty = true; }
		}

		/// <summary>
		/// shortcut to entity.transform.rotation
		/// </summary>
		/// <value>The rotation.</value>
		public float Rotation
		{
			get => _rotation;
			set { _rotation = value; _areMatrixesDirty = true; _areBoundsDirty = true; }
		}

		/// <summary>
		/// the zoom value should be between -1 and 1. This value is then translated to be from minimumZoom to maximumZoom. This lets you set
		/// appropriate minimum/maximum values then use a more intuitive -1 to 1 mapping to change the zoom.
		/// </summary>
		/// <value>The zoom.</value>
		public float Zoom
		{
			get => _zoom;
			set { _zoom = value; _areBoundsDirty = true; _areMatrixesDirty = true; }
		}
		/// <summary>
		/// world-space bounds of the camera. useful for culling.
		/// </summary>
		/// <value>The bounds.</value>
		public RectangleF Bounds
		{
			get
			{
				if (_areMatrixesDirty)
					UpdateMatrixes();

				if (_areBoundsDirty)
				{
					// top-left and bottom-right are needed by either rotated or non-rotated bounds
					var topLeft = ScreenToWorldPoint(new Vector2(Core.GraphicsDevice.Viewport.X,
						Core.GraphicsDevice.Viewport.Y));
					var bottomRight = ScreenToWorldPoint(new Vector2(
						Core.GraphicsDevice.Viewport.X + Core.GraphicsDevice.Viewport.Width,
						Core.GraphicsDevice.Viewport.Y + Core.GraphicsDevice.Viewport.Height));

					if (Rotation != 0)
					{
						// special care for rotated bounds. we need to find our absolute min/max values and create the bounds from that
						var topRight = ScreenToWorldPoint(new Vector2(
							Core.GraphicsDevice.Viewport.X + Core.GraphicsDevice.Viewport.Width,
							Core.GraphicsDevice.Viewport.Y));
						var bottomLeft = ScreenToWorldPoint(new Vector2(Core.GraphicsDevice.Viewport.X,
							Core.GraphicsDevice.Viewport.Y + Core.GraphicsDevice.Viewport.Height));

						var minX = Mathf.MinOf(topLeft.X, bottomRight.X, topRight.X, bottomLeft.X);
						var maxX = Mathf.MaxOf(topLeft.X, bottomRight.X, topRight.X, bottomLeft.X);
						var minY = Mathf.MinOf(topLeft.Y, bottomRight.Y, topRight.Y, bottomLeft.Y);
						var maxY = Mathf.MaxOf(topLeft.Y, bottomRight.Y, topRight.Y, bottomLeft.Y);

						_bounds.Location = new Vector2(minX, minY);
						_bounds.Width = maxX - minX;
						_bounds.Height = maxY - minY;
					}
					else
					{
						_bounds.Location = topLeft;
						_bounds.Width = bottomRight.X - topLeft.X;
						_bounds.Height = bottomRight.Y - topLeft.Y;
					}

					_areBoundsDirty = false;
				}

				return _bounds;
			}
		}

		/// <summary>
		/// used to convert from world coordinates to screen
		/// </summary>
		/// <value>The transform matrix.</value>
		public Matrix2D TransformMatrix
		{
			get
			{
				if (_areMatrixesDirty)
					UpdateMatrixes();
				return _transformMatrix;
			}
		}

		/// <summary>
		/// used to convert from screen coordinates to world
		/// </summary>
		/// <value>The inverse transform matrix.</value>
		public Matrix2D InverseTransformMatrix
		{
			get
			{
				if (_areMatrixesDirty)
					UpdateMatrixes();
				return _inverseTransformMatrix;
			}
		}

		/// <summary>
		/// the 2D Cameras projection matrix
		/// </summary>
		/// <value>The projection matrix.</value>
		public Matrix ProjectionMatrix
		{
			get
			{
				if (_isProjectionMatrixDirty)
				{
					Matrix.CreateOrthographicOffCenter(0, Core.GraphicsDevice.Viewport.Width,
						Core.GraphicsDevice.Viewport.Height, 0, 0, -1, out _projectionMatrix);
					_isProjectionMatrixDirty = false;
				}

				return _projectionMatrix;
			}
		}

		/// <summary>
		/// gets the view-projection matrix which is the transformMatrix * the projection matrix
		/// </summary>
		/// <value>The view projection matrix.</value>
		public Matrix ViewProjectionMatrix => TransformMatrix * ProjectionMatrix;

		public Vector2 Origin
		{
			get => _origin;
			internal set
			{
				if (_origin != value)
				{
					_origin = value;
					_areMatrixesDirty = true;
				}
			}
		}

		Vector2 _position;
		float _zoom;
		float _rotation;
		RectangleF _bounds;
		Matrix2D _transformMatrix = Matrix2D.Identity;
		Matrix2D _inverseTransformMatrix = Matrix2D.Identity;
		Matrix _projectionMatrix;
		Vector2 _origin;

		bool _areMatrixesDirty = true;
		bool _areBoundsDirty = true;
		bool _isProjectionMatrixDirty = true;

		public Camera()
        {
			Position = new Vector2(0, 0);
			Rotation = 0;
			Zoom = 1f;
        }

		protected virtual void UpdateMatrixes()
		{
			if (!_areMatrixesDirty)
				return;

			Matrix2D tempMat;
			_transformMatrix =
				Matrix2D.CreateTranslation(-_position.X, -_position.Y); // position

			if (_zoom != 1f)
			{
				Matrix2D.CreateScale(_zoom, _zoom, out tempMat); // scale ->
				Matrix2D.Multiply(ref _transformMatrix, ref tempMat, out _transformMatrix);
			}

			if (_rotation != 0f)
			{
				Matrix2D.CreateRotation(_rotation, out tempMat); // rotation
				Matrix2D.Multiply(ref _transformMatrix, ref tempMat, out _transformMatrix);
			}

			Matrix2D.CreateTranslation((int)_origin.X, (int)_origin.Y, out tempMat); // translate -origin
			Matrix2D.Multiply(ref _transformMatrix, ref tempMat, out _transformMatrix);

			// calculate our inverse as well
			Matrix2D.Invert(ref _transformMatrix, out _inverseTransformMatrix);

			// whenever the matrix changes the bounds are then invalid
			_areBoundsDirty = true;
			_areMatrixesDirty = false;
		}

		/// <summary>
		/// this forces the matrix and bounds dirty
		/// </summary>
		public void ForceMatrixUpdate()
		{
			// dirtying the matrix will automatically dirty the bounds as well
			_areMatrixesDirty = true;
		}

		#region transformations

		/// <summary>
		/// converts a point from world coordinates to screen
		/// </summary>
		/// <returns>The to screen point.</returns>
		/// <param name="worldPosition">World position.</param>
		public Vector2 WorldToScreenPoint(Vector2 worldPosition)
		{
			UpdateMatrixes();
			Vector2Ext.Transform(ref worldPosition, ref _transformMatrix, out worldPosition);
			return worldPosition;
		}


		/// <summary>
		/// converts a point from screen coordinates to world
		/// </summary>
		/// <returns>The to world point.</returns>
		/// <param name="screenPosition">Screen position.</param>
		public Vector2 ScreenToWorldPoint(Vector2 screenPosition)
		{
			UpdateMatrixes();
			Vector2Ext.Transform(ref screenPosition, ref _inverseTransformMatrix, out screenPosition);
			return screenPosition;
		}


		/// <summary>
		/// converts a point from screen coordinates to world
		/// </summary>
		/// <returns>The to world point.</returns>
		/// <param name="screenPosition">Screen position.</param>
		public Vector2 ScreenToWorldPoint(Point screenPosition)
		{
			return ScreenToWorldPoint(screenPosition.ToVector2());
		}


		public Vector2 ToLateral(Vector2 l)
        {
			var co = Mathf.Cos(-Rotation);
			var si = Mathf.Sin(-Rotation);
			return new Vector2(co * l.X - si * l.Y, si * l.X + co * l.Y);
        }


		/// <summary>
		/// returns the mouse position in world space
		/// </summary>
		/// <returns>The to world point.</returns>
		public Vector2 MouseToWorldPoint()
		{
			return ScreenToWorldPoint(Input.MousePosition);
		}

		#endregion
	}
}