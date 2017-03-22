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
        private Model ball;

        private World world;

        private PhysikEngine engine;

        private Camera camera;

        private Vector2 mousePos;

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
            camera = new Camera(new Vector3(0, 0, 2000), new Vector3(0,-0.2f,-1), MathHelper.PiOver4,
                (float) graphics.PreferredBackBufferWidth / (float) graphics.PreferredBackBufferHeight, 1.0f, 10000.0f,
                5.0f);
            mousePos = Mouse.GetState().Position.ToVector2();

            world = new World(2000f, this.GraphicsDevice);

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

            engine = new PhysikEngine(9.81f, world,true);

            engine.AddObject(new Ball(ball,Vector3.Zero,Vector3.Zero, true, 1,1));

            var r = new Random();
            for (int i = 0; i < 50; i++)
            {
                engine.AddObject(new Ball(ball, new Vector3(r.Next(-100, 100), r.Next(-100, 100), r.Next(-100, 100)),
                    new Vector3(r.Next(10, 100), r.Next(10, 100), r.Next(10, 100)), false,
                    0.95f, (float)(r.Next(10, 100) + r.NextDouble())));
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
            camera.UpdateCamera();
            engine.Update(gameTime);

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

            base.Draw(gameTime);
        }
    }
}