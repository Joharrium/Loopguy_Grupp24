using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    class Checkbox : Component
    {
        Texture2D check, uncheck;
        bool state;
        string text;
        //yep
        public bool State
        {
            get { return state; }
        }

        Vector2 position;

        public override bool isHovering { get; set; }

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, 300, 20);
            }
        }


        public Checkbox(Vector2 position, string text)
        {
            this.position = position - new Vector2(30, 0);
            state = false;
            uncheck = TextureManager.checkbox_false;
            check = TextureManager.checkbox_true;
            this.text = text;
        }

        public override void Update(GameTime gameTime)
        {
            //if (isHovering)
            {
            //    Check();
            }
        }

        public bool Check()
        {
            if(InputReader.ButtonPressed(Buttons.A) ||InputReader.LeftClick() || InputReader.KeyPressed(Keys.Enter))
            {
                state = !state;
            }
            return state;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isHovering)
            {
                spriteBatch.Draw(TextureManager.UI_selectedMenuBox, Rectangle, Color.White);
                

            }

            if(state)
            {
                spriteBatch.Draw(check, position + new Vector2(0, 0), Color.White);
            }
            else
            {
                spriteBatch.Draw(uncheck, position + new Vector2(0, 0), Color.White);
            }
            
            
            spriteBatch.DrawString(TextureManager.UI_menuFont, text, position + new Vector2(40, 0), Color.White);



        }
    }
}
