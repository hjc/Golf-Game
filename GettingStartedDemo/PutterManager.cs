using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Entities;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.MathExtensions;


namespace GettingStartedDemo 
{
    class PutterManager : DrawableGameComponent
    {
        /// <summary>
        /// Entity that this model follows.
        /// </summary>
        Model model;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;

        private float xRot = -MathHelper.PiOver4;

        //Matrices for scaling and moving the putter
        private Matrix scaleMat;
        private Matrix transMat;

        //rotation Z matrix is the same everytime, used to orient club
        private static Matrix zRotMat = Matrix.CreateRotationZ(MathHelper.PiOver2);

        /// <summary>
        /// These variables are used to push the putter forward. To tell it when to push, how long to
        ///  and how long it has pushed.
        /// </summary>
        private bool push = false;
        private float pushTimer;
        private float pushElapsed = 0;
        private float pushRot = 0;

         /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public PutterManager(Model model, Matrix transform, Matrix scale, Matrix translate, Game game)
            : base(game)
        {
            this.model = model;
            this.Transform = transform;
            this.scaleMat = scale;
            this.transMat = translate;

            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the EntityModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
            }
        }

        public void StartPushing(float pushAmount)
        {
            this.push = true;
            this.pushTimer = pushAmount;
            this.pushElapsed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Left))
                xRot -= 0.01f;
            if (kbState.IsKeyDown(Keys.Right))
                xRot += 0.01f;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // effect.World = Matrix.CreateScale(0.1f) * Transform;
                    // effect.World = Matrix.CreateRotationX(MathHelper.PiOver4) * Transform;
                    effect.World = boneTransforms[mesh.ParentBone.Index];

                    //now orient the putter right
                    effect.World *= zRotMat;

                    

                    //now rotate putter
                    effect.World *= Matrix.CreateRotationY(xRot) /** Matrix.CreateRotationX(xRot)*/;

                    //first scale model
                    effect.World *= scaleMat;

                    //now translate the putter
                    effect.World *= transMat;

                    
                    effect.World *= Transform;

                    if (push)
                    {
                        pushElapsed += gameTime.ElapsedGameTime.Milliseconds;
                        if (pushElapsed <= pushTimer)
                        {
                            pushRot += 0.01f;
                            effect.World *= Matrix.CreateTranslation(new Vector3(pushRot, 0, 0));
                        }
                        else
                        {
                            push = false;
                            pushRot = 0;
                        }
                    }

                    
                    effect.View = (Game as GettingStartedGame).Camera.ViewMatrix;
                    effect.Projection = (Game as GettingStartedGame).Camera.ProjectionMatrix;

                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
