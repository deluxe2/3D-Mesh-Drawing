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

        private Model cube;
        private Model ball;


        private Matrix world;

        private Camera camera;

        private Vector2 mousePos;

        private Vector2 center;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            world = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            camera = new Camera(new Vector3(0, 0, 10), Vector3.Zero, MathHelper.PiOver4,
                (float) graphics.PreferredBackBufferWidth / (float) graphics.PreferredBackBufferHeight, 0.1f, 1000.0f,
                1.0f);
            mousePos = Mouse.GetState().Position.ToVector2();

            center = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

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
            cube = Content.Load<Model>("cube");


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
            UpdateCamera(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            foreach (ModelMesh mesh in ball.Meshes)
            {
                foreach (var effect1 in mesh.Effects)
                {
                    var effect = (BasicEffect) effect1;
                    effect.World = world;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }

                mesh.Draw();
            }

            foreach (var mesh in cube.Meshes)
            {
                foreach (var effect1 in mesh.Effects)
                {
                    var effect = (BasicEffect)effect1;
                    effect.World = Matrix.CreateScale(10f)* world;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }

        private void UpdateCamera(GameTime gameTime)
        {
            var lastmousepos = new Vector2(mousePos.X,mousePos.Y);
            mousePos = Mouse.GetState().Position.ToVector2();

            var cameraPosition = camera.Position;
            var cameraTarget = camera.Target;

            

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed && mousePos != center)
            {
                var mousedif = lastmousepos - mousePos;
                cameraTarget = Vector3.Transform(camera.Target,
                    Matrix.CreateRotationY(MathHelper.ToRadians(mousedif.X *(graphics.PreferredBackBufferWidth / 2.0f) / 180)) * Matrix.CreateRotationX(MathHelper.ToRadians(mousedif.Y * (graphics.PreferredBackBufferHeight / 2.0f) / 180)));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cameraPosition += GetMovingVector(Matrix.CreateRotationY(0)) * camera.CameraSpeed;
                cameraTarget += GetMovingVector(Matrix.CreateRotationY(0)) * camera.CameraSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraPosition += GetMovingVector(Matrix.CreateRotationY(MathHelper.Pi)) * camera.CameraSpeed;
                cameraTarget += GetMovingVector(Matrix.CreateRotationY(MathHelper.Pi)) * camera.CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cameraPosition += GetMovingVector(Matrix.CreateRotationY(MathHelper.Pi/2)) * camera.CameraSpeed;
                cameraTarget += GetMovingVector(Matrix.CreateRotationY(MathHelper.Pi/2)) * camera.CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraPosition += GetMovingVector(Matrix.CreateRotationY(MathHelper.Pi/-2)) * camera.CameraSpeed;
                cameraTarget += GetMovingVector(Matrix.CreateRotationY(MathHelper.Pi/-2)) * camera.CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                cameraPosition += GetMovingVector(Matrix.CreateRotationX(MathHelper.Pi/2)) * camera.CameraSpeed;
                cameraTarget += GetMovingVector(Matrix.CreateRotationX(MathHelper.Pi/2)) * camera.CameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                cameraPosition += GetMovingVector(Matrix.CreateRotationX(MathHelper.Pi / -2)) * camera.CameraSpeed;
                cameraTarget += GetMovingVector(Matrix.CreateRotationX(MathHelper.Pi / -2)) * camera.CameraSpeed;
            }

            camera.UpdatePositions(cameraPosition, cameraTarget);

        }

        private Vector3 GetMovingVector(Matrix rotation)
        {

                return Vector3.Transform( Vector3.Normalize(camera.Target-camera.Position),rotation) ;

        }
    }
}