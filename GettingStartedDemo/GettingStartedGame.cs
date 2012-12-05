using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.MathExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using System.Collections.Generic;


namespace GettingStartedDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GettingStartedGame : Microsoft.Xna.Framework.Game
    {
        //store variables for putting

        //power variables
        int currentPower = 5;
        int maxPower = 10;
        GraphicsDeviceManager graphics;
        /// <summary>
        /// World in which the simulation runs.
        /// </summary>
        Space space;
        /// <summary>
        /// Controls the viewpoint and how the user can see the world.
        /// </summary>
        public Camera Camera;
        /// <summary>
        /// Graphical model to use for the boxes in the scene.
        /// </summary>
        public Model CubeModel;
        /// <summary>
        /// Graphical model to use for the environment.
        /// </summary>
        public Model PlaygroundModel;

        float elapsedTime = 0;

        /// Starting positions
        /*public Vector3[] startingPos = { new Vector3(-3.77f, -16.19f, 10.75f),
                                         new Vector3(0.05f,  -17.14f, 4.25f),
                                         new Vector3(3.25f,  -17.17f, 4.25f),
                                         new Vector3(5.75f,  -15.75f, 6.1f),
                                         new Vector3(9.1f,   -16.95f, 7.5f),
                                         new Vector3(15.2f,  -13.7f,  6.1f),
                                         new Vector3(22.5f,  -10.2f,  5.2f)};*/

        public Vector3[] startingPos = { new Vector3(-3.77f, -16.19f, 10.70f),
                                         new Vector3(0.05f,  -17.14f, 4.20f),
                                         new Vector3(3.25f,  -17.17f, 4.20f),
                                         new Vector3(5.75f,  -15.75f, 6.1f),
                                         new Vector3(9.1f,   -16.95f, 7.45f),
                                         new Vector3(15.2f,  -13.7f,  6.05f),
                                         new Vector3(22.5f,  -10.2f,  5.15f)};

        //var to hold our current level, used to fetch balls
        private int level = 0;

        // list to hold all our balls
        public List<Entity> balls;

        /// <summary>
        /// Contains the latest snapshot of the keyboard's input state.
        /// </summary>
        public KeyboardState KeyboardState;
        /// <summary>
        /// Contains the latest snapshot of the mouse's input state.
        /// </summary>
        public MouseState MouseState;

        // camera offset
        private static Vector3 backOffset = new Vector3(0, -2, -2);

        //prevent space bar spam
        private float noLevelSpam = 0;
        private float noLevelSpamCap = 1000;

        //prevent level reset spam
        private float noResetSpam = 0;
        private float noResetSpamCap = 1000;


        public GettingStartedGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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
            //Setup the camera.
            Camera = new Camera(this, new Vector3(0, 3, 10), 5);

            

            this.balls = new List<Entity>();

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        //add a model to scene - a LoadContent Helper method
        public void AddModelLevel(Model model, bool isStatic = false) {
            Vector3[] vertices;
            int[] indices;
            //Matrix.CreateScale(1.0f);
            //Matrix.CreateRotationX(MathHelper.PiOver4);
            TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
            
            
            //model is static: should not be affected by gravity

            //Give the mesh information to a new StaticMesh.  
            //Give it a transformation which scoots it down below the kinematic box entity we created earlier.
            var mesh = new StaticMesh(vertices, indices, new AffineTransform( new Vector3(0, -20, 0)));
           
           
            //Add it to the space!
            space.Add(mesh);

            //Make it visible too.
            Components.Add(new StaticModel(model, mesh.WorldTransform.Matrix, this));
        }
        protected override void LoadContent()
        {
            //This 1x1x1 cube model will represent the box entities in the space.
            CubeModel = Content.Load<Model>("cube");

            PlaygroundModel = Content.Load<Model>("playground");
         
            
            //Construct a new space for the physics simulation to occur within.
            space = new Space();

            Sphere[] toAdd = new Sphere[7];

            // Initialize the game: one ball on each court.
            for (int i = 0; i < 7; i++)
            {
                toAdd[i] = new Sphere(startingPos[i], 0.2f);

                //Set the velocity of the new box to fly in the direction the camera is pointing.
                //Entities have a whole bunch of properties that can be read from and written to.
                //Try looking around in the entity's available properties to get an idea of what is available.
                // toAdd.LinearVelocity = Camera.WorldMatrix.Forward * 10;
                toAdd[i].LinearVelocity = Vector3.Zero;
                //Add the new box to the simulation.
                space.Add(toAdd[i]);
                balls.Add(toAdd[i]);



                // Add a graphical representation of the box to the drawable game components.
                //StaticModel model = new StaticModel(CubeModel, Matrix.CreateScale(0.5f), this);
                EntityModel model = new EntityModel(toAdd[i], CubeModel, Matrix.CreateScale(0.2f), this);

                Components.Add(model);
                toAdd[i].Tag = model;  //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.
            }


            //Set the gravity of the simulation by accessing the simulation settings of the space.
            //It defaults to (0,0,0); this changes it to an 'earth like' gravity.
            //Try looking around in the space's simulationSettings to familiarize yourself with the various options.
            space.ForceUpdater.Gravity = new Vector3(0, -9.81f*2, 0);

            //Make a box representing the ground and add it to the space.
            //The Box is an "Entity," the main simulation object type of BEPUphysics.
            //Examples of other entities include cones, spheres, cylinders, and a bunch more (a full listing is in the BEPUphysics.Entities namespace).

            //Every entity has a set of constructors.  Some half a parameter for mass, others don't.
            //Constructors that allow the user to specify a mass create 'dynamic' entiites which fall, bounce around, and generally work like expected.
            //Constructors that have no mass parameter create a create 'kinematic' entities.  These can be thought of as having infinite mass.
            //This box being added is representing the ground, so the width and length are extended and it is kinematic.

            Sphere ground = new Sphere((Vector3.Zero), 1, 30);
            space.Add(ground);

            

            //Robert 2 : ADD YOUR PREVIOUSLY DECLARED MODeL HERE using AddModelLevel: 
            //AddModelLevel(Snowman);
            Model Levels;
            //Robert 2. 
            Levels = Content.Load<Model>("holes1");
            AddModelLevel(Levels);

            Matrix[] transforms = new Matrix[Levels.Bones.Count];
            Levels.CopyAbsoluteBoneTransformsTo(transforms);


            //Go through the list of entities in the space and create a graphical representation for them.
            foreach (Entity e in space.Entities)
            {
                Box box = e as Box;
                if (box != null) //This won't create any graphics for an entity that isn't a box since the model being used is a box.
                {
                    //Matrix scaling = Matrix.CreateScale(0.1f);
                    Matrix scaling = Matrix.CreateScale(box.Width, box.Height, box.Length); //Since the cube model is 1x1x1, it needs to be scaled to match the size of each individual box.
                    EntityModel model = new EntityModel(e, CubeModel, scaling, this);
                    //Add the drawable game component for this entity to the game.
                    Components.Add(model);
                    e.Tag = model; //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.
                }
            }

            setupLevel();
        }

        /// <summary>
        /// Used to handle a collision event triggered by an entity specified above.
        /// </summary>
        /// <param name="sender">Entity that had an event hooked.</param>
        /// <param name="other">Entity causing the event to be triggered.</param>
        /// <param name="pair">Collision pair between the two objects in the event.</param>
        void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            //This type of event can occur when an entity hits any other object which can be collided with.
            //They aren't always entities; for example, hitting a StaticMesh would trigger this.
            //Entities use EntityCollidables as collision proxies; see if the thing we hit is one.
            var otherEntityInformation = other as EntityCollidable;
            if (otherEntityInformation != null)
            {
                //We hit an entity! remove it.
                space.Remove(otherEntityInformation.Entity); 
                //Remove the graphics too.
                Components.Remove((EntityModel)otherEntityInformation.Entity.Tag);
            }
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //sets up our next level
        private void setupLevel()
        {
            //Camera.setWorldMatrix(Matrix.CreateLookAt(Camera.Position, startingPos[level], Vector3.Up));
            Camera.setPosition(balls[level].Position - backOffset, startingPos[level]);
            balls[level].Mass = 1f;
        }

        //TODO: USE A STROKE WHEN YOU MULLIGAN
        private void resetLevel()
        {
            //store ref to old ball
            Entity t = balls[level];
            
            //create new ball on physics space, store it
            Sphere newBall = new Sphere(startingPos[level], 0.2f);
            newBall.LinearVelocity = Vector3.Zero;
            space.Add(newBall);
            balls[level] = newBall;

            //create new model
            EntityModel model = new EntityModel(newBall, CubeModel, Matrix.CreateScale(0.2f), this);
            Components.Add(model);
            balls[level].Tag = model;

            //clean up old ball
            space.Remove(balls[level]);

            setupLevel();
        }

        //this moves us to the next level and then calls setup level
        private void updateLevel()
        {
            level++;
            if (level >= balls.Count)
                level = 0;
            setupLevel();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //Update the camera.
            Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);

            #region CONTROLS

            float ms = gameTime.ElapsedGameTime.Milliseconds;

            //deal with shooting the balls
            elapsedTime += ms;
            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                
               // if mouse is pressed, velocity change

               // If the user is clicking, start firing some boxes.
               // First, create a new dynamic box at the camera's location.

                if(elapsedTime >=1000)
                {
                   balls[level].ApplyImpulse(balls[level].Position, new Vector3(-Camera.ViewMatrix.Forward.X, Camera.ViewMatrix.Forward.Y, Camera.ViewMatrix.Forward.Z) * 10);
                   elapsedTime = 0;
                   Camera.ballMotionOn();
                 }
            }

            //deal with going to next level
            noLevelSpam += ms;
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                if (noLevelSpam >= noLevelSpamCap) {
                    noLevelSpam = 0;
                    updateLevel();
                    Camera.ballMotionOff();
                }

            }

            //deal with resetting the ball
            noResetSpam += ms;
            if (KeyboardState.IsKeyDown(Keys.P))
            {
                if (noResetSpam >= noResetSpamCap)
                {
                    noResetSpam = 0;
                    resetLevel();
                    Camera.ballMotionOff();
                }
            }


            #endregion

            //kbState = Keyboard.GetState();

            //add code to increase power, rotate putter

            //Steps the simulation forward one time step.
            space.Update();

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

            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called whenever we need to remove a model or unload a level.
        /// </summary>

        public void removeModel(ISpaceObject m)
        {
            space.Remove(m);
        }

        //add an object to the space
        public void addToSpace(ISpaceObject obj)
        {
            space.Add(obj);
        }

        //add a static model to components (easier done here because we have "this")
        public void addStaticModel(Model m, StaticMesh mesh)
        {
            Components.Add(new StaticModel(m, mesh.WorldTransform.Matrix, this));
        }

        public int getLevel()
        {
            return level;
        }
    }
}
