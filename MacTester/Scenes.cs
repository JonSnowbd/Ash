﻿using System;
using Nez;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Textures;
using Nez.Tiled;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.Overlap2D;
using Nez.LibGdxAtlases;
using Nez.BitmapFonts;
using System.Collections.Generic;
using System.IO;
using Nez.Particles;


namespace MacTester
{
	public static class Scenes
	{
		public static Scene sceneOne( bool useScalingViewportAdapter = true )
		{
			var scene = Scene.createWithDefaultRenderer( Color.Black );

			if( useScalingViewportAdapter )
				scene.camera.viewportAdapter = new ScalingViewportAdapter( Core.graphicsDevice, 256, 144 );
			else
				scene.camera.viewportAdapter = new BoxingViewportAdapter( Core.graphicsDevice, 256, 144 );
			scene.camera.centerOrigin();

			Screen.preferredBackBufferWidth = 256 * 4; 
			Screen.preferredBackBufferHeight = 144 * 4;
			Screen.applyChanges();

			// load a TiledMap and move it back so is drawn before other entities
			var tiledEntity = scene.createAndAddEntity<Entity>( "tiled-map-entity" );
			var tiledmap = scene.contentManager.Load<TiledMap>( "bin/MacOSX/Tilemap/tilemap" );
			tiledEntity.addComponent( new TiledMapComponent( tiledmap, "collision" ) );
			tiledEntity.order += 5;


			// create a sprite animation from an atlas
			var plumeTexture = scene.contentManager.Load<Texture2D>( "Images/plume" );
			var subtextures = Subtexture.subtexturesFromAtlas( plumeTexture, 16, 16 );
			var spriteAnimation = new SpriteAnimation( subtextures )
			{
				loop = true,
				fps = 10
			};

			var sprite = new Sprite<int>( subtextures[0] );
			sprite.addAnimation( 0, spriteAnimation );
			sprite.play( 0 );

			var spriteEntity = scene.createAndAddEntity<Entity>( "sprite-dude" );
			spriteEntity.position = new Vector2( 40, 40 );
			spriteEntity.addComponent( sprite );

			return scene;
		}


		public static Scene sceneTwo()
		{
			var scene = new Scene();
			scene.clearColor = Color.Coral;
			scene.camera.centerOrigin();
			var moonTexture = scene.contentManager.Load<Texture2D>( "Images/moon" );
			var bmFont = scene.contentManager.Load<BitmapFont>( "bin/MacOSX/Fonts/pixelfont" );
			bmFont.spacing = 2f;

			// setup a renderer that renders everything to a RenderTexture making sure its order is before standard renderers!
			var renderer = new DefaultRenderer( scene.camera, -1 );
			renderer.renderTexture = new RenderTexture( 320, 240 );
			renderer.renderTextureClearColor = Color.CornflowerBlue;
			scene.addRenderer( renderer );

			// add a standard renderer that renders to the screen
			scene.addRenderer( new DefaultRenderer() );

			// stick a couple moons on screen
			var entity = scene.createAndAddEntity<Entity>( "moon" );
			var image = new Image( moonTexture );
			image.originNormalized = Vector2Ext.halfVector();
			image.zoom = 2f;
			entity.addComponent( image );
			entity.addComponent( new FramesPerSecondCounter( Graphics.instance.bitmapFont, Color.White, FramesPerSecondCounter.FPSDockPosition.TopLeft ) );
			entity.position = new Vector2( 120f, 0f );
			entity.collider = new CircleCollider( moonTexture.Width * 1.5f );


			entity = scene.createAndAddEntity<Entity>( "new-moon" );
			image = new Image( moonTexture );
			entity.position = new Vector2( 130f, 130f );
			entity.addComponent( image );


			entity = scene.createAndAddEntity<Entity>( "bmfont" );
			entity.addComponent( new Text( Graphics.instance.bitmapFont, "This text is a BMFont\nPOOOP", new Vector2( 0, 30 ), Color.Red ) );
			entity.addComponent( new Text( bmFont, "This text is a BMFont\nPOOOP", new Vector2( 0, 70 ), Color.Cornsilk ) );


			// texture atlas tester
			var anotherAtlas = scene.contentManager.Load<TextureAtlas>( "bin/MacOSX/TextureAtlasTest/AnotherAtlas" );
			var textureAtlas = scene.contentManager.Load<TextureAtlas>( "bin/MacOSX/TextureAtlasTest/AtlasImages" );

			entity = scene.createAndAddEntity<Entity>( "texture-atlas-sprite" );
			entity.position = new Vector2( 30f, 330f );

			// create a sprite animation from an atlas
			var spriteAnimation = new SpriteAnimation()
			{
				loop = true,
				fps = 10
			};
			spriteAnimation.addFrame( textureAtlas.getSubtexture( "Ninja_Idle_0" ) );
			spriteAnimation.addFrame( textureAtlas.getSubtexture( "Ninja_Idle_1" ) );
			spriteAnimation.addFrame( textureAtlas.getSubtexture( "Ninja_Idle_2" ) );
			spriteAnimation.addFrame( textureAtlas.getSubtexture( "Ninja_Idle_3" ) );
			spriteAnimation.addFrame( anotherAtlas.getSubtexture( "Ninja_Air Dash_0" ) );
			spriteAnimation.addFrame( anotherAtlas.getSubtexture( "Ninja_Air Dash_1" ) );
			spriteAnimation.addFrame( anotherAtlas.getSubtexture( "Ninja_Air Dash_2" ) );
			spriteAnimation.addFrame( anotherAtlas.getSubtexture( "Ninja_Air Dash_3" ) );

			var sprite = new Sprite<int>( 1, anotherAtlas.getSpriteAnimation( "hardLanding" ) );
			sprite.addAnimation( 0, spriteAnimation );
			sprite.play( 1 );
			entity.addComponent( sprite );


			// add a post processor to display the RenderTexture
			var effect = scene.contentManager.LoadEffect( "Content/Effects/Invert.ogl.mgfxo" );
			var postProcessor = new SimplePostProcessor( renderer.renderTexture, effect );
			scene.addPostProcessor( postProcessor );
			scene.enablePostProcessing = true;

			return scene;
		}


		public static Scene sceneThree( bool useBoxColliders = true )
		{
			var scene = Scene.createWithDefaultRenderer( useBoxColliders ? Color.BlanchedAlmond : Color.Azure );
			var moonTexture = scene.contentManager.Load<Texture2D>( "Images/moon" );


			// create some moons
			Action<Vector2,string,bool> moonMaker = ( Vector2 pos, string name, bool isTrigger ) =>
			{
				var ent = scene.createAndAddEntity<Entity>( name );
				ent.position = pos;
				ent.addComponent( new Image( moonTexture ) );
				if( useBoxColliders )
					ent.collider = new BoxCollider();
				else
					ent.collider = new CircleCollider();

				ent.collider.isTrigger = isTrigger;
			};

			moonMaker( new Vector2( 400, 10 ), "moon1", false );
			moonMaker( new Vector2( 10, 10 ), "moon2", false );
			moonMaker( new Vector2( 50, 500 ), "moon3", true );
			moonMaker( new Vector2( 500, 250 ), "moon4", false );


			// add an animation to "moon4" to test moving collisions
			scene.findEntity( "moon4" ).addComponent( new SimpleMovingPlatform( 250, 400 ) );

			// create a player moon
			var entity = scene.createAndAddEntity<Entity>( "player-moon" );
			entity.addComponent( new SimpleMoonMover() );
			entity.position = new Vector2( 220, 220 );
			var sprite = new Sprite<int>( new Subtexture( moonTexture ) );
			sprite.color = Color.Blue;
			entity.addComponent( sprite );

			if( useBoxColliders )
				entity.collider = new BoxCollider();
			else
				entity.collider = new CircleCollider();


			var uglyBackgroundEntity = scene.createAndAddEntity<Entity>( "bg" );
			uglyBackgroundEntity.order = 5;
			var image = new Image( scene.contentManager.Load<Texture2D>( "Images/dots-512" ) );
			image.zoom = 4f;
			uglyBackgroundEntity.addComponent( image );


			// add a follow camera
			var camFollow = scene.createAndAddEntity<Entity>( "camera-follow" );
			camFollow.addComponent( new FollowCamera( entity ) );
			camFollow.addComponent( new CameraShake() );

			return scene;
		}


		public static Scene sceneOverlap2D()
		{
			var scene = Scene.createWithDefaultRenderer( Color.Aquamarine );
			scene.camera.centerOrigin();

			var sceneEntity = scene.createAndAddEntity<Entity>( "overlap2d-scene-entity" );
			var o2ds = scene.contentManager.Load<O2DScene>( "bin/MacOSX/Overlap2D/MainScene" );
			var sceneTexture = scene.contentManager.Load<LibGdxAtlas>( "bin/MacOSX/Overlap2D/packatlas" );
			foreach( var si in o2ds.sImages )
			{
				try
				{
					var i = new Image( sceneTexture.getSubtexture( si.imageName ) );
					i.localPosition = new Vector2( si.x, -si.y );
					i.origin = new Vector2( si.originX, si.originY );
					i.scale = new Vector2( si.scaleX, si.scaleY );
					sceneEntity.addComponent( i );
				}
				catch( Exception )
				{
				}
			}


			return scene;
		}


		public static Scene sceneFour()
		{
			var scene = Scene.createWithDefaultRenderer( Color.Aquamarine );
			scene.camera.centerOrigin();
			var moonTexture = scene.contentManager.Load<Texture2D>( "Images/moon" );

			var entity = scene.createAndAddEntity<Entity>( "moon" );
			var image = new Image( moonTexture );
			image.originNormalized = Vector2Ext.halfVector();
			entity.addComponent( image );
			entity.position = new Vector2( 200, 200 );

			var points = new List<Vector2>();
			points.Add( new Vector2( -50, -50 ) );
			points.Add( new Vector2( 0, -70 ) );
			points.Add( new Vector2( 50, -40 ) );
			points.Add( new Vector2( 50, 50 ) );
			points.Add( new Vector2( -50, 50 ) );
			points.Add( new Vector2( -50, -50 ) );
			entity.collider = new PolygonCollider( points.ToArray() );
			( entity.collider as PolygonCollider ).rotation = MathHelper.PiOver2;


			entity = scene.createAndAddEntity<Entity>( "moon2" );
			image = new Image( moonTexture );
			image.originNormalized = Vector2Ext.halfVector();
			entity.addComponent( image );
			entity.position = new Vector2( 500, 500 );

			points = new List<Vector2>();
			points.Add( new Vector2( -50, -50 ) );
			points.Add( new Vector2( 50, 30 ) );
			points.Add( new Vector2( -40, 40 ) );
			points.Add( new Vector2( -50, -50 ) );

			entity.collider = new PolygonCollider( points.ToArray() );
			entity.collider = new OrientedBoxCollider( 128, 128 );
			( entity.collider as PolygonCollider ).rotation = MathHelper.PiOver4;
			entity.addComponent( new SimpleMoonMover() );


			entity = scene.createAndAddEntity<Entity>( "moon2" );
			image = new Image( moonTexture );
			image.originNormalized = Vector2Ext.halfVector();
			entity.addComponent( image );
			entity.position = new Vector2( 700, 300 );
			entity.collider = new OrientedBoxCollider( 128, 128 );
			( entity.collider as OrientedBoxCollider ).rotation = MathHelper.PiOver4 + 0.1f;

			return scene;
		}


		static int lastEmitter = 0;
		public static Scene sceneFive()
		{
			var scene = Scene.createWithDefaultRenderer( Color.Black );
			scene.camera.centerOrigin();

			var particles = new string[]
			{
				"bin/MacOSX/ParticleDesigner/Fire",
				"bin/MacOSX/ParticleDesigner/Snow",
				"bin/MacOSX/ParticleDesigner/Leaves",
				"bin/MacOSX/ParticleDesigner/Atomic Bubble",
				"bin/MacOSX/ParticleDesigner/Blue Flame",
				"bin/MacOSX/ParticleDesigner/Blue Galaxy",
				"bin/MacOSX/ParticleDesigner/Comet",
				"bin/MacOSX/ParticleDesigner/Crazy Blue",
				"bin/MacOSX/ParticleDesigner/Electrons",
				"bin/MacOSX/ParticleDesigner/Giros Gratis",
				"bin/MacOSX/ParticleDesigner/huo1",
				"bin/MacOSX/ParticleDesigner/Into The Blue",
				"bin/MacOSX/ParticleDesigner/JasonChoi_Flash",
				"bin/MacOSX/ParticleDesigner/JasonChoi_rising up",
				"bin/MacOSX/ParticleDesigner/JasonChoi_Swirl01",
				"bin/MacOSX/ParticleDesigner/Meks Blood Spill",
				"bin/MacOSX/ParticleDesigner/Plasma Glow",
				"bin/MacOSX/ParticleDesigner/Real Popcorn",
				"bin/MacOSX/ParticleDesigner/Shooting Fireball",
				"bin/MacOSX/ParticleDesigner/The Sun",
				"bin/MacOSX/ParticleDesigner/Touch Up",
				"bin/MacOSX/ParticleDesigner/Trippy",
				"bin/MacOSX/ParticleDesigner/Winner Stars",
				"bin/MacOSX/ParticleDesigner/wu1"
			};

			if( lastEmitter == particles.Length )
				lastEmitter = 0;
			var whichEmitter = particles[lastEmitter++];


			var entity = scene.createAndAddEntity<Entity>( "particles" );
			entity.position = new Vector2( Screen.backBufferWidth / 2, Screen.backBufferHeight / 2 );
			var particleEmitterConfig = scene.contentManager.Load<ParticleEmitterConfig>( whichEmitter );
			entity.addComponent( new ParticleEmitter( particleEmitterConfig ) );


			entity = scene.createAndAddEntity<Entity>( "text" );
			var textComp = new Text( Graphics.instance.bitmapFont, whichEmitter, new Vector2( 0, 0 ), Color.White );
			textComp.scale = new Vector2( 2, 2 );
			textComp.origin = Vector2.Zero;
			entity.addComponent( textComp );

			return scene;
		}

	}
}

