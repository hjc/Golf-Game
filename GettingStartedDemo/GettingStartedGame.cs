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
using BEPUphysics.CollisionShapes;


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
        

        //hold keyboard state for us
        KeyboardState kbState = Keyboard.GetState();

        /// <summary>
        /// This manages all of our various levels.
        /// Will load their models initially and handle cycling through them
        ///  and loading them into the game.
        /// Only need one
        /// </summary>
        protected LevelManager LevelMan;

        /// <summary>
        /// Manage our Putter for us. Keep a reference to pass events
        /// </summary>
        private PutterManager puttMan;

        /// <summary>
        /// Putter timer for elapsedTime
        /// </summary>
        private float putterElapsed = 1000;

        PowerScore powerscore = new PowerScore();
        float elapsedTime = 0;
        int power;
        int strokecount;
        //SpriteFont spfont;
        //SpriteBatch spbatch;

        // Starting positions
        public Vector3[] startingPos = { new Vector3(-3.77f, -16.19f, 10.75f),
                                         new Vector3(0.05f,  -17.14f, 4.25f),
                                         new Vector3(3.25f,  -17.17f, 4.25f),
                                         new Vector3(5.75f,  -15.75f, 6.1f),
                                         new Vector3(9.1f,   -16.95f, 7.5f),
                                         new Vector3(15.2f,  -13.7f,  6.1f),
                                         new Vector3(22.5f,  -10.2f,  5.2f)};


        


#if XBOX360
        /// <summary>
        /// Contains the latest snapshot of the gamepad's input state.
        /// </summary>
        public GamePadState GamePadState;
#else
        /// <summary>
        /// Contains the latest snapshot of the keyboard's input state.
        /// </summary>
        public KeyboardState KeyboardState;
        /// <summary>
        /// Contains the latest snapshot of the mouse's input state.
        /// </summary>
        public MouseState MouseState;
#endif



        public GettingStartedGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            this.LevelMan = new LevelManager();
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

            power = 5;
            strokecount = 0;

           
            powerscore.ShowInTaskbar = false;
            powerscore.Show();
            
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

        public void AddPutter(Model model)
        {
            Vector3[] vertices;
            int[] indices;
            //Matrix.CreateScale(1.0f);
            //Matrix.CreateRotationX(MathHelper.PiOver4);
            TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
            //CapsuleObject
           

            //model is static: should not be affected by gravity
            //Box putterHead = new Box(new Vector3(1, -13.5f, 2), 4f, 4f, 4f);

            Box putterHead = new Box(new Vector3(1, 1, 1), 4f, 4f, 4f);
            //StaticMesh putterHead = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -20, 0)));

           // putterHead.IsAffectedByGravity = false;
            //putterHead.

            //ConvexHull putterHead = new ConvexHull(Vector3.Zero, vertices);
            
            //StaticMesh putterHead = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, 0, 0)));
           
            //putterHead.LinearVelocity = Vector3.Zero;//(5f, 5f, -15f);

            //Sphere putterHead = new Sphere(Vector3.Zero, 1f);

            space.Add(putterHead);
            //putterHead.BecomeKinematic();
            //putterHead.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;

            //Give the mesh information to a new StaticMesh.  
            //Give it a transformation which scoots it down below the kinematic box entity we created earlier.
            //var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -20, 0)));

            
            //Add it to the space!
            //space.Add(mesh);
            //this.puttMan = new PutterManager(putterHead, model, mesh.WorldTransform.Matrix, Matrix.CreateScale(0.05f), Matrix.CreateTranslation(new Vector3(1, 1, 1)), this);
            //this.puttMan = new PutterManager(putterHead, model, Matrix.Identity, Matrix.Identity, Matrix.Identity, this);
            this.puttMan = new PutterManager(putterHead, model, Matrix.Identity, Matrix.CreateScale(0.05f), Matrix.CreateTranslation(new Vector3(-5f, -5f, -5f)), this);
            //this.puttMan = new PutterManager(putterHead, model, Matrix.Identity, Matrix.Identity, Matrix.Identity, this);

            //EntityModel em = new EntityModel(putterHead, model, putterHead.WorldTransform, this);

            putterHead.Tag = this.puttMan;

            //Make it visible too.
            Components.Add(this.puttMan);
        }

         protected override void LoadContent()
         {

            //spfont = Content.Load<SpriteFont>(@"Arial");
            //spbatch = new SpriteBatch(graphics.GraphicsDevice);
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


            //Now that we have something to fall on, make a few more boxes.
            //These need to be dynamic, so give them a mass- in this case, 1 will be fine.
            //space.Add(new Box(new Vector3(0, 4, 0), 1, 1, 1, 1));
            //space.Add(new Box(new Vector3(0, 8, 0), 1, 1, 1, 1));
            //space.Add(new Sphere(new Vector3(3,0,2), 1,1));
            //Create a physical environment from a triangle mesh.
            //First, collect the the mesh data from the model using a helper function.
            //This special kind of vertex inherits from the TriangleMeshVertex and optionally includes
            //friction/bounciness data.
            //The StaticTriangleGroup requires that this special vertex type is used in lieu of a normal TriangleMeshVertex array.
          //  Vector3[] vertices;
           // int[] indices;
           // TriangleMesh.GetVerticesAndIndicesFromModel(Snowman, out vertices, out indices);
            //Give the mesh information to a new StaticMesh.  
            //Give it a transformation which scoots it down below the kinematic box entity we created earlier.
            //var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -20, 0)));

            //Add it to the space!
            //space.Add(mesh);
            //Make it visible too.
            //Components.Add(new StaticModel(Snowman, mesh.WorldTransform.Matrix, this));
            

            //Robert 2 : ADD YOUR PREVIOUSLY DECLARED MODeL HERE using AddModelLevel: 
            //AddModelLevel(Snowman);
            Model Levels;
            //Robert 2. 
            Levels = Content.Load<Model>("holes1");
            AddModelLevel(Levels);

            Matrix[] transforms = new Matrix[Levels.Bones.Count];
            Levels.CopyAbsoluteBoneTransformsTo(transforms);

            Model Putter;
            Putter = Content.Load<Model>("putter");
            AddPutter(Putter);
          
           
            //Hook an event hModelLevel to an entity to handle some game logic.
            //Refer to the Entity Events documentation for more information.
            /*Sphere deleterBox = new Sphere(new Vector3(5, 2, 0), 3);
            space.Add(deleterBox);
            deleterBox.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;*/


            //Go through the list of entities in the space and create a graphical representation for them.

             //dont really need this
           /* foreach (Entity e in space.Entities)
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
            }*/
            //this.LevelMan.setupCurrentLevel();
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

            //var otherEntityInformation = other as Entity;
            //if (otherEntityInformation != null)
            //{
                //We hit an entity! remove it.
                space.Remove(otherEntityInformation.Entity); 
                //Remove the graphics too.
                Components.Remove((EntityModel)otherEntityInformation.Entity.Tag);
            //}
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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

            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //Update the camera.
            Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            

            #region Block shooting

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            //power adjustment
            powerscore.show_power(power);
            if (elapsedTime >= 100)
            {
                if (KeyboardState.IsKeyDown(Keys.Up))
                {
                    power++;
                    
                    if (power > 10)
                        power = 10;
                    elapsedTime = 0;
                }
                if (KeyboardState.IsKeyDown(Keys.Down))
                {
                    power--;
                    if (power < 1)
                        power = 1;
                    
                    elapsedTime = 0;
                }
            }

            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                
               // if mouse is pressed, velocity change

               // If the user is clicking, start firing some boxes.
               // First, create a new dynamic box at the camera's location.
                if(elapsedTime >=1000)
                {
                    //Sphere toAdd = new Sphere(Camera.Position, 0.2f, 1);
                   
                
                    //Set the velocity of the new box to fly in the direction the camera is pointing.
                    //Entities have a whole bunch of properties that can be read from and written to.
                    //Try looking around in the entity's available properties to get an idea of what is available.
                    //toAdd.LinearVelocity = Camera.WorldMatrix.Forward * 10;
                    //Add the new box to the simulation.
                    //space.Add(toAdd);

                   // Add a graphical representation of the box to the drawable game components.
                    //StaticModel model = new StaticModel(CubeModel, Matrix.CreateScale(0.5f), this);
                    //EntityModel model = new EntityModel(toAdd, CubeModel, Matrix.CreateScale(0.1f), this);
                    //Components.Add(model);
                    //toAdd.Tag = model;  //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.

                    
            
                   elapsedTime = 0;
                }
            }

            #endregion
            kbState = Keyboard.GetState();

            putterElapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (kbState.IsKeyDown(Keys.Space))
            {
                if (putterElapsed >= 1000)
                {
                    puttMan.StartPushing(1000);

                    strokecount++;
                    powerscore.show_stroke(strokecount);

                    putterElapsed = 0;
                }

            }

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
            //spbatch.Begin();
            //spbatch.DrawString(spfont, "Power: " + power.ToString(), new Vector2(20.0f, 20.0f), Color.Black);
            //spbatch.End();
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
    }
}
