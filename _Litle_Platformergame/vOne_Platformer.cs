
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class vOne_Platformer : _GameRunSetup
{
    //använder camera2d

    /*
        + Göra så star och slut är implimenterat.
        + varje gång du stänger builder så startar du vid start.
        + när kameran är roterad ska minrecrot va roterad också, du ska också inte kuna rotera mitt i ändring.
            - så när pressed. ingen left eller right.
        + gravitation på spelaren.
        + Jump funktion. o lite annat kul.
        + en death plane. en rectangel som tjäckar så du är inuti den. kan göras på andra sätt.

    */

    public vOne_Platformer(Camera2D camera2D,GraphicsDeviceManager _graphics, SpriteBatch _spriteBatch, Texture2D texture) : base(camera2D,_graphics,_spriteBatch,texture)
    {
        Lmin = SaveLoadRotRec.Load();
    }
    Du Player = new Du(20,20);
    List<MinRotRect> Lmin;
    Build_rec_camera2D build_Rec;
    void BuilderMethod(Keys ÖppnaBuilder,Keys StängBuilder)
    {
        if (KeyboardHelper.IsKeyDown(ÖppnaBuilder))
        {
            build_Rec = new Build_rec_camera2D(camera2D,Lmin);
        }
        if (KeyboardHelper.IsKeyDown(StängBuilder))
        {
            build_Rec=null;
            camera2D.Rotation=0;
            camera2D.Zoom=1;
        }
        if (build_Rec != null)
        {
            camera2D.Move();
            build_Rec.Camera2D=camera2D;
            build_Rec.Update();
            Lmin=build_Rec.ListOfRec;
        }
        if (KeyboardHelper.IsKeyDown(Keys.Escape))
        {
            SaveLoadRotRec.Save(Lmin);
        }
    }
    public override void Update(GameTime gameTime)
    {
        if(build_Rec==null){
            camera2D.Pos=Player.centrum;
        }
        BuilderMethod(Keys.N,Keys.C);
        if (build_Rec == null)
        {
            Player.Update();
        }
    }









    public override void Draw()
    {
        if(build_Rec!=null){
            build_Rec.Draw(_spriteBatch,texture);
        }
        _spriteBatch.Begin(transformMatrix:camera2D.get_transformation());
        if (build_Rec == null)
        {
            foreach (MinRotRect rec in Lmin)
            {    
                _spriteBatch.Draw(texture,rec.centrum,null,new Color(rec.centrum_x/1800,0,rec.centrum_y/1000),rec.rotation,Vector2.One/2,new Vector2(rec.width / (float)texture.Width,rec.height / (float)texture.Height),SpriteEffects.None,0f);   
            }
        }
        _spriteBatch.Draw(texture,Player.centrum,null,Color.Aqua,Player.rotation,Vector2.One/2,new Vector2(Player.width / (float)texture.Width,Player.height / (float)texture.Height),SpriteEffects.None,0f);   

        _spriteBatch.End();
        
    }
}
