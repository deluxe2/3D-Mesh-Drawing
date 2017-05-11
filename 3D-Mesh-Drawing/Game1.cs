using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DHelper;

namespace _3D_Mesh_Drawing
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont font;
        private Texture2D fadenkreuz;

        private Model ball;
        private Model plane;

        private World world;

        private PhysikEngine engine;

        private Camera camera;

        private bool shotFired;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(new Vector3(0, 0, 9000), Vector3.Forward, MathHelper.PiOver4,
                (float) graphics.PreferredBackBufferWidth / (float) graphics.PreferredBackBufferHeight, 1.0f, 100000.0f,
                50.0f);

            world = new World(10000f, this.GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ball = Content.Load<Model>("ball");
            plane = Content.Load<Model>("FracturedPlane");
            font = Content.Load<SpriteFont>("Font");
            fadenkreuz = Content.Load<Texture2D>("Fadenkreuz");

            engine = new PhysikEngine(9.81f, world);

            var r = new Random();
            for (int i = 0; i < 50; i++)
            {
                engine.AddObject(new Ball(ball, new Vector3(r.Next(-100, 100), r.Next(-100, 100), r.Next(-100, 100)),
                    new Vector3(r.Next(10, 1000), r.Next(10, 1000), r.Next(10, 1000)), false,
                    0.90f, (float) (r.Next(10, 200) + r.NextDouble())));
            }


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Shrinks World
            //if (world.CubeSize > 2000f)
            //{
            //    world.CubeSize -= 5f;
            //}

            camera.UpdateCamera();
            engine.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                shotFired = true;
            }
            else
            {
                if (shotFired)
                {
                    engine.AddObject(new Ball(ball, camera.Position,
                        Vector3.Normalize(camera.Target - camera.Position) * 100f, false, 0.95f, 150));
                    shotFired = false;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            // TODO: Add your drawing code here
            engine.Draw(camera);

            world.DrawWorld(camera);

            camera.Draw(spriteBatch, font, fadenkreuz,
                new Vector2(graphics.PreferredBackBufferWidth / 2 - 16, graphics.PreferredBackBufferHeight / 2 - 16));

            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
        }
    }
}