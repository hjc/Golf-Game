using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace GettingStartedDemo
{
    /// <summary>
    /// Basic camera class supporting mouse/keyboard/gamepad-based movement.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Determine if we're debugging (turned on with ~). Debugging allows free camera
        ///   movement with esdf and az
        /// </summary>
        /// 
        private bool DEBUG = false;
        private bool tildePressed = false;
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }
        float yaw;
        float pitch;
        /// <summary>
        /// Vars to help us rotate our camera around the ball. Needs its own speed
        /// </summary>
        float rot = 0f;
        float rotSpeed = 0.1f;
        private Vector3 cameraTarget;
        private float rotResetTimer = 0;
        private static float rotResetMax = 100;

        ///Deal with moving the camera when ball is hit
        private bool inMotion = false;

        //prevent ball from moving for next putt
        private bool holdMotion = false;
        /// <summary>
        /// Gets or sets the yaw rotation of the camera.
        /// </summary>
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = MathHelper.WrapAngle(value);
            }
        }
        /// <summary>
        /// Gets or sets the pitch rotation of the camera.
        /// </summary>
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = MathHelper.Clamp(value, -MathHelper.PiOver2, MathHelper.PiOver2);
            }
        }

        /// <summary>
        /// Gets or sets the speed at which the camera moves.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix { get; private set; }
        /// <summary>
        /// Gets or sets the projection matrix of the camera.
        /// </summary>
        public Matrix ProjectionMatrix { get; set; }

        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix { get; private set; }

        /// <summary>
        /// Gets the game owning the camera.
        /// </summary>
        public GettingStartedGame Game { get; private set; }

        /// <summary>
        /// Constructs a new camera.
        /// </summary>
        /// <param name="game">Game that this camera belongs to.</param>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="speed">Initial movement speed of the camera.</param>
        public Camera(GettingStartedGame game, Vector3 position, float speed)
        {
            Game = game;
            Position = position;
            Speed = speed;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4f / 3f, .1f, 10000.0f);
            Mouse.SetPosition(200, 200);
        }

        /// <summary>
        /// Moves the camera forward using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void MoveForward(float dt)
        {
            Position += WorldMatrix.Forward * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera right using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveRight(float dt)
        {
            Position += WorldMatrix.Right * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera up using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveUp(float dt)
        {
            Position += new Vector3(0, (dt * Speed), 0);
        }

        public void setWorldMatrix(Matrix newWorld)
        {
            WorldMatrix = newWorld;
        }

        /// <summary>
        /// Updates the camera's view matrix.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void Update(float dt, GameTime gameTime)
        {
            #region DEBUG
            //DEBUG code. Turns debug on and off and the controls it enables
            if (DEBUG)
            {
                //Turn based on mouse input.
                Yaw += (200 - Game.MouseState.X) * dt * .12f;
                Pitch += (200 - Game.MouseState.Y) * dt * .12f;
                Mouse.SetPosition(200, 200);
            }

            // let's make tilde a toggle. Little more code, but no math.
            // This code turns Debugging on or off, which lets us do noclip
            if (Game.KeyboardState.IsKeyDown(Keys.OemTilde))
            {
                if (!this.tildePressed)
                {
                    this.DEBUG = !this.DEBUG;
                    this.tildePressed = true;
                }
            }
            else
                this.tildePressed = false;

            //Scoot the camera around depending on what keys are pressed.
            if (DEBUG)
            {
                float distance = Speed * dt;
                if (Game.KeyboardState.IsKeyDown(Keys.E))
                    MoveForward(distance);
                if (Game.KeyboardState.IsKeyDown(Keys.D))
                    MoveForward(-distance);
                if (Game.KeyboardState.IsKeyDown(Keys.S))
                    MoveRight(-distance);
                if (Game.KeyboardState.IsKeyDown(Keys.F))
                    MoveRight(distance);
                if (Game.KeyboardState.IsKeyDown(Keys.A))
                    MoveUp(distance);
                if (Game.KeyboardState.IsKeyDown(Keys.Z))
                    MoveUp(-distance);
            }
            #endregion

            float rotDistance = rotSpeed * dt;

            //controls for moving the camera
            if (Game.KeyboardState.IsKeyDown(Keys.Left))
                RotateLeft(-rotDistance);
            if (Game.KeyboardState.IsKeyDown(Keys.Right))
                RotateLeft(rotDistance);
            if (Game.KeyboardState.IsKeyDown(Keys.Down))
                Position += new Vector3(0, -rotDistance * 10, 0);
            if (Game.KeyboardState.IsKeyDown(Keys.Up))
                Position += new Vector3(0, rotDistance * 10, 0);


            //ball in motion? if so track it
            if (inMotion)
            {
                int level = Game.getLevel();
                Vector3 linVel = Game.balls[level].LinearVelocity;
                if (Math.Abs(linVel.X) < 0.5 && Math.Abs(linVel.Y) < 0.5 && Math.Abs(linVel.Z) < 0.5)
                {
                    Game.balls[level].LinearVelocity = Vector3.Zero;
                    inMotion = false;
                    holdMotion = true;
                }

                // change camera position to track ball
                Position = Game.balls[level].Position + new Vector3(0, 2, 2);// -new Vector3(0, Position.Y, 0);
                cameraTarget = Game.balls[level].Position;

                //setup general camera orientation
                WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);

                //move camera to the ball
                WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);

                //make camera look at ball
                ViewMatrix = Matrix.CreateLookAt(Position, cameraTarget, Vector3.Up);
            }
            else
            {
                //use this to keep the ball stopped after its been hit and is coming
                // to a stop
                if (holdMotion)
                    Game.balls[Game.getLevel()].LinearVelocity = Vector3.Zero;
                //setup general camera orientation
                WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);

                //move camera to ball
                WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);

                

                //setup position to rotate around ball
                Position = Vector3.Transform(Position - cameraTarget, Matrix.CreateFromAxisAngle(Vector3.Up, rot)) + cameraTarget;

                //we have to reset the rotation to 0 or else the camera rotates forever (no
                // ese bueno). Do this every so often to give the illusion of full control
                rotResetTimer += gameTime.ElapsedGameTime.Milliseconds;

                //make camera rotate around ball
                if (rot != 0)
                    if (rotResetTimer > rotResetMax)
                    {
                        rot = 0;
                        rotResetTimer = 0;
                    }
                //make camera look at ball
                ViewMatrix = Matrix.CreateLookAt(Position, cameraTarget, Vector3.Up);
            }

            //ViewMatrix *= Matrix.Invert(WorldMatrix);
        }

        private void RotateLeft(float distance)
        {
            this.rot += distance;
        }

        public void setPosition(Vector3 newPos, Vector3 newTarget)
        {
            Position = newPos;
            cameraTarget = newTarget;
            Yaw = -0.0145915207f;
            Pitch = -0.7507962f;
        }

        public void ballMotionOn()
        {
            this.inMotion = true;
        }

        public void ballMotionOff()
        {
            this.inMotion = false;
        }

        public void stopHoldMotion()
        {
            this.holdMotion = false;
        }
    }
}
