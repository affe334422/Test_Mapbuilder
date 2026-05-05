
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Platform_builderV1 : _GameRunSetup
{
    public Platform_builderV1(Camera2D camera2D,GraphicsDeviceManager _graphics, SpriteBatch _spriteBatch, Texture2D texture) : base(camera2D,_graphics,_spriteBatch,texture)
    {
        Lmin = SaveLoadRotRec.Load();
    }
    List<LitenStudsBoll> LitenBoll = new List<LitenStudsBoll>();
    List<MinRotRect> Lmin;
    Build_rec build_Rec;
    public override void Update(GameTime gameTime)
    {
        LitenBoll.ForEach(L=>L.Update(Lmin));
        if (KeyboardHelper.IsKeyDown(Keys.N))
        {
            build_Rec = new Build_rec(Lmin);
        }
        if (KeyboardHelper.IsKeyDown(Keys.C))
        {
            build_Rec=null;
        }
        if (build_Rec != null)
        {
            build_Rec.Update();
            Lmin=build_Rec.ListOfRec;
        }
        else if(MouseHelper.isPressed())
        {
            LitenBoll.Add(new LitenStudsBoll(MouseHelper.CurretPosition(),10,10));
        }
        if (KeyboardHelper.IsKeyDown(Keys.Escape))
        {
            SaveLoadRotRec.Save(Lmin);
        }
        
    }
    public override void Draw()
    {
        if(build_Rec!=null){
            build_Rec.Draw(_spriteBatch,texture);
        }
        _spriteBatch.Begin();
        if (build_Rec == null)
        {
            foreach (MinRotRect rec in Lmin)
            {
                _spriteBatch.Draw(texture,rec.centrum,null,new Color(rec.centrum_x/1800,0,rec.centrum_y/1000),rec.rotation,Vector2.One/2,new Vector2(rec.width / (float)texture.Width,rec.height / (float)texture.Height),SpriteEffects.None,0f);
            }
        }
        LitenBoll.ForEach(L=>_spriteBatch.Draw(texture,L.rec,Color.DarkCyan));

        _spriteBatch.End();
        
    }
}
