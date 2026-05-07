
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Start_and_Finish
{
    public Start_and_Finish()
    {
        
    }
    private MinRotRect Start;
    public MinRotRect SetStart
    {
        set
        {
            Start=value;
            FixPlaySpace();
        }
    }
    public Vector2 StartingPoint{get=>Start.centrum;}
    private MinRotRect Finish;
    public MinRotRect SetFinish{set=>Finish=value;}
    private MinRotRect PlayableSpace;
    public MinRotRect SetPlaySpace
    {
        set
        {
            PlayableSpace = value;
            FixPlaySpace();
        }
    }
    private void FixPlaySpace()
    {
        if (!PlayableSpace.Contains(Start.centrum)&&PlayableSpace!=null&&Start!=null)
        {
            Vector2 NewSize = PlayableSpace.centrum-Start.centrum;
            NewSize = new Vector2(Math.Abs(NewSize.X),Math.Abs(NewSize.Y));
            PlayableSpace.ChangeSize((int)(NewSize.X+0.5),(int)(NewSize.Y+0.5));
        }
    }

    public bool YouWin(MinRotRect Player)
    {
        foreach(Vector2 Hörn in Player.hörn)
        {
            if(Finish.Contains(Hörn)){return true;}
        }
        return false;
    }
    public void Draw(SpriteBatch _spriteBatch, Texture2D texture,Camera2D camera2D)
    {
        _spriteBatch.Begin(transformMatrix:camera2D.get_transformation());
        if (Start != null)
        {
            _spriteBatch.Draw(texture,Start.centrum,null,new Color(Start.centrum_x/1800,0,Start.centrum_y/1000),Start.rotation,Vector2.One/2,new Vector2(Start.width / (float)texture.Width,Start.height / (float)texture.Height),SpriteEffects.None,0f);
        }
        if (Finish != null)
        {
            _spriteBatch.Draw(texture,Finish.centrum,null,new Color(Finish.centrum_x/1800,0,Finish.centrum_y/1000),Finish.rotation,Vector2.One/2,new Vector2(Finish.width / (float)texture.Width,Finish.height / (float)texture.Height),SpriteEffects.None,0f);
        }
        _spriteBatch.End();
    }
}
