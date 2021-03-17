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

namespace HelloZune
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont tahomaFont;
        MediaLibrary sampleMediaLibrary;
        Random rand;
        String SongName = " ";
        String AlbumName = " ";
        String ArtistName = " ";
        Vector2 FontPos;
        Vector2 ArtPos;
        Vector2 AlbPos;
        Vector2 VolPos;
        Vector2 AlbArtPos;
        Texture2D background;
        float CurVol;
        int TrackNum;
        bool isPaused = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sampleMediaLibrary = new MediaLibrary();
            rand = new Random();
            // Frame rate is 30 fps by default for Zune.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
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

            base.Initialize(); 
            SongName = "Press Right for Random";
           

           
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
          //  soundEffect = Content.Load<SoundEffect>(@"Audio\CameraShutter");
            tahomaFont = Content.Load<SpriteFont>("Tahoma");
            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 5,
        graphics.GraphicsDevice.Viewport.Height / 2);
            ArtPos = new Vector2(0, 30);
            VolPos = new Vector2(185, 30);
            AlbPos = new Vector2(0, 0);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
            {
                
                MediaPlayer.Stop();
                // generate a random valid index into Albums
                int i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
                int s = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count );
                // play the first track from the album
                MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[s]);

                // set the song name
                ArtistName = sampleMediaLibrary.Albums[i].Artist.ToString();
                AlbumName = sampleMediaLibrary.Albums[i].ToString();
                SongName = sampleMediaLibrary.Albums[i].Songs[s].ToString();
                background = sampleMediaLibrary.Albums[i].GetAlbumArt(this.Services);
                TrackNum = s;
                // Reset Positions
                FontPos = new Vector2(50, 300);
                ArtPos = new Vector2(10, 30);
                AlbPos = new Vector2(0, 0);
                AlbArtPos = new Vector2(0,50);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                // forgot to set this up to continue music. 
                if (!isPaused)
                {
                MediaPlayer.Pause();
                isPaused = true;
                }
                else {
                    MediaPlayer.Resume();
                    isPaused = false;
                    
                }
               
                

            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
            {
                
               CurVol = Convert.ToSingle(MediaPlayer.Volume);
               MediaPlayer.Volume = CurVol + .1f;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
            {
                CurVol = Convert.ToSingle(MediaPlayer.Volume);
                MediaPlayer.Volume = CurVol - .1f;
            }
            //check if song is done
           

            if (SongName.Length >= 25)
            {
                if (FontPos.X == -100)
                {
                    FontPos.X = 100;
                }
                else
                {
                    FontPos.X = FontPos.X - 1;
                }

            }
            if (AlbumName.Length >= 25)
            {
                if (AlbPos.X == -125)
                {
                    AlbPos.X = 125;
                }
                else
                {
                    AlbPos.X = AlbPos.X - 1;
                }

            }
            if (ArtistName.Length >= 25)
            {
                if (ArtPos.X == -125)
                {
                    ArtPos.X = 125;
                }
                else
                {
                    ArtPos.X = ArtPos.X - 1;
                }

            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (background != null ) {
                spriteBatch.Draw(background, AlbArtPos, Color.White);
           
            }

            spriteBatch.DrawString(tahomaFont, ArtistName, ArtPos, Color.White);
            spriteBatch.DrawString(tahomaFont, "Vol:" + CurVol, VolPos, Color.White);
            spriteBatch.DrawString(tahomaFont, AlbumName, AlbPos, Color.White);
            spriteBatch.DrawString(tahomaFont, TrackNum + ". " + SongName, FontPos, Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
