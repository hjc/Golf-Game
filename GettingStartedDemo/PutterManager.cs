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

         /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public PutterManager(Model model, Matrix transform, Game game)
            : base(game)
        {
            this.model = model;
            this.Transform = transform;

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
                    //now rotate putter
                    effect.World *= Matrix.CreateRotationX(xRot);
                    effect.World *= Transform;

                    
                    effect.View = (Game as GettingStartedGame).Camera.ViewMatrix;
                    effect.Projection = (Game as GettingStartedGame).Camera.ProjectionMatrix;

                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
