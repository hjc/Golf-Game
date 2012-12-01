using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BEPUphysics.Entities;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables;
using BEPUphysics.MathExtensions;

namespace GettingStartedDemo
{
    public class LevelManager : DrawableGameComponent
    {
      //  private List<Model> Levels = new List<Model>();

        GettingStartedGame theGame;
 
        private int CurrentLevel = 0;

        private StaticMesh CurrentLevelEntity;

        //beat a level; get ready for the next by incrementing current level
        // and then loading the next
        //private void nextLevel() {
        //    CurrentLevel += 1;

        //    if (CurrentLevel > Levels.Count)
        //        //win
        //        return;
        //    else
        //    {
        //        //first remove current level
        //        this.theGame.removeModel(CurrentLevelEntity);
        //        //load next level

        //        //set level to current level
        //        return;
        //    }
        //}

        protected override void LoadContent()
        {
            //// Load explosion stuff.
            //explosionTexture =
            //    Game.Content.Load<Texture2D>(@"textures\particle");
            //explosionEffect =
            //    Game.Content.Load<Effect>(@"effects\particle");
            //explosionEffect.CurrentTechnique =
            //    explosionEffect.Techniques["Technique1"];
            //explosionEffect.Parameters["theTexture"].SetValue(
            //    explosionTexture);

            //// Load star texture and effect.
            //starTexture = Game.Content.Load<Texture2D>(@"textures\stars");
            //starEffect = explosionEffect.Clone(GraphicsDevice);
            //starEffect.CurrentTechnique = starEffect.Techniques["Technique1"];
            //starEffect.Parameters["theTexture"].SetValue(starTexture);

            //// Initialize particle star sheet.
            //stars = new ParticleStarSheet(
            //    GraphicsDevice,
            //    new Vector3(2000, 2000, -1900),
            //    1500,
            //    new Vector2(starTexture.Width, starTexture.Height),
            //    particleSettings);

            Model Snowman;
            //Robert 2. 
            Snowman = Game.Content.Load<Model>("set2");

       
            AddModelLevel(Snowman);

            base.LoadContent();
        }

        //public void setupCurrentLevel()
        //{
        //    AddModelLevel(this.Levels[0]);
        //}

        public LevelManager(Game game, GettingStartedGame game2) : base(game)
        {
            this.theGame = game2;
        }

        //add a model to scene - a LoadContent Helper method
        public void AddModelLevel(Model model, bool isStatic = false)
        {
            Vector3[] vertices;
            int[] indices;
            TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);


            //model is static: should not be affected by gravity

            //Give the mesh information to a new StaticMesh.  
            //Give it a transformation which scoots it down below the kinematic box entity we created earlier.
            var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -20, 0)));

            //Add it to the space!
            this.theGame.addToSpace(mesh);
            //Make it visible too.
            this.theGame.addStaticModel(model, mesh);

            this.CurrentLevelEntity = mesh;
        }
    }
}
