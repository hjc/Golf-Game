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
        Entity entity;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;

        //variable to rotate the putter into the right spot
        private float xRot = 0;

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
        /// This is used to determine where the putter is and will determine how the
        ///   putt action is done.
        /// </summary>
        private Vector3 forwardPos = new Vector3(0.5f, 0.5f, 0);

         /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public PutterManager(Entity entity, Model model, Matrix transform, Matrix scale, Matrix translate, Game game)
            : base(game)
        {
            this.entity = entity;
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
            {
                xRot -= 0.01f;
                //forwardPos.X
                forwardPos.X += 0.005f;
                forwardPos.Y -= 0.005f;
            }
            if (kbState.IsKeyDown(Keys.Right))
            {
                xRot += 0.01f;
                forwardPos.X -= 0.005f;
                forwardPos.Y += 0.005f;
            }
            base.Update(gameTime);
        }

        //public override void Draw(GameTime gameTime)
        //{
        //    //Notice that the entity's worldTransform property is being accessed here.
        //    //This property is returns a rigid transformation representing the orientation
        //    //and translation of the entity combined.
        //    //There are a variety of properties available in the entity, try looking around
        //    //in the list to familiarize yourself with it.
        //    Matrix worldMatrix = Transform * entity.WorldTransform;


        //    model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        //    foreach (ModelMesh mesh in model.Meshes)
        //    {
        //        foreach (BasicEffect effect in mesh.Effects)
        //        {

        //            effect.World = boneTransforms[mesh.ParentBone.Index] * worldMatrix;
        //            effect.View = (Game as GettingStartedGame).Camera.ViewMatrix;
        //            effect.Projection = (Game as GettingStartedGame).Camera.ProjectionMatrix;
        //        }
        //        mesh.Draw();
        //    }
        //    base.Draw(gameTime);
        //}

        public override void Draw(GameTime gameTime)
        {

            Matrix worldMatrix = Transform * entity.WorldTransform;

            //entity.Position;
            Matrix trans = Matrix.CreateTranslation(entity.Position);

            Matrix w = Matrix.CreateWorld(entity.Position, new Vector3(1, 0, 0), new Vector3(0, 1, 0));



            Matrix w2 = Matrix.CreateScale(0.05f) * zRotMat * Matrix.CreateTranslation(entity.Position);

            entity.WorldTransform *= Matrix.CreateScale(0.05f);
            
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // effect.World = Matrix.CreateScale(0.1f) * Transform;
                    // effect.World = Matrix.CreateRotationX(MathHelper.PiOver4) * Transform;
                    effect.World = boneTransforms[mesh.ParentBone.Index] /* * worldMatrix*/;

                    //now rotate putter
                    //effect.World *= Matrix.CreateRotationX(xRot) /* * Matrix.CreateRotationX(xRot)*/;

                    //now orient the putter right
                    //effect.World *= zRotMat;





                    //first scale model
                    //effect.World *= scaleMat;

                    //now translate the putter
                    //effect.World *= transMat;


                    //effect.World *= Transform;
                    effect.World *= w2;
                    //effect.World *= entity.WorldTransform;
                    if (push)
                    {
                        pushElapsed += gameTime.ElapsedGameTime.Milliseconds;
                        if (pushElapsed <= pushTimer)
                        {
                            pushRot += 0.01f;
                            effect.World *= Matrix.CreateTranslation(new Vector3(pushRot, pushRot, 0) * forwardPos);
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
