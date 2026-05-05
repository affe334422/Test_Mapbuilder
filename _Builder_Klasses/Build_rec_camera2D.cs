

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Build_rec_camera2D : Build_rec
{
    private Camera2D camera2D;
    public Build_rec_camera2D(Camera2D camera2D,List<MinRotRect> Lmin) : base(Lmin)
    {
        this.camera2D = camera2D;
    }
    public Camera2D Camera2D
    {
        set{
            camera2D=value;
            currMouse = camera2D.get_mouse_pos_with_cam(MouseHelper.CurretPosition());
            prevMouse = camera2D.get_mouse_pos_with_cam(MouseHelper.PreviousPosition());
        }
    }
    
    
    protected override void BuildANewRec()
    {
        if (IsItANewRecs&&MouseHelper.Click())
        {
            StartPos = currMouse;
            Lmin.Add(new MinRotRect(StartPos, 1, 1));
            IsItANewRecs = false;
        }
        else
        {
            if (MouseHelper.Click())
            {
                IsItANewRecs = true;
            }
        }
        if(!IsItANewRecs){
            if(Lmin.Count>0)
            {
                Vector2 current = currMouse;
                int x = (int)Math.Min(StartPos.X, current.X);
                int y = (int)Math.Min(StartPos.Y, current.Y);
                int width = (int)Math.Abs(current.X - StartPos.X);
                int height = (int)Math.Abs(current.Y - StartPos.Y);
                Lmin.Last().ChangeSize(width,height);
                Lmin.Last().centrum = new Vector2(x+width/2,y+height/2);
            }
        }
    }
    protected override void DeleteInBuilder()
    {
        if (KeyboardHelper.AllKeysUp()&&KeyboardHelper.IsKeyDown(Keys.Delete)&&Lmin.Count>0)
        {
            Lmin.Remove(Lmin.Last());
            IsItANewRecs=true;
        }
    }
    protected override void ChooseARec()
    {
        foreach(MinRotRect min in Lmin)
        {
            if (min.Contains(currMouse)&&MouseHelper.RightClick())
            {
                SelectedRec = min;
            }
        }   
    }
    protected override void ReSizeOrMove()
    {
        if(!MouseHelper.isPressed()){
            MoveEveryting=false;
            RotateIt=false;
            corner = SelectedRec.VilketHörnRörDu(currMouse);
        }
        else if (corner != -1)
        {
            int opposite = (corner + 2) % 4;

            Vector2 fixedCorner = SelectedRec.hörn[opposite];

            Vector2 center = (fixedCorner + currMouse) / 2f;

            Vector2 axisX = new Vector2(
                (float)Math.Cos(SelectedRec.rotation),
                (float)Math.Sin(SelectedRec.rotation));

            Vector2 axisY = new Vector2(-axisX.Y, axisX.X);

            Vector2 diff = currMouse - fixedCorner;

            float width = Math.Abs(Vector2.Dot(diff, axisX));
            float height = Math.Abs(Vector2.Dot(diff, axisY));

            SelectedRec.centrum = center;
            SelectedRec.ChangeSize((int)(width+0.5), (int)(height+0.5));
        }
        else if ((SelectedRec.Contains(currMouse) && MouseHelper.isPressed() && !RotateIt) || (MoveEveryting && !RotateIt))
        {
            MoveEveryting=true;
            SelectedRec.centrum+=currMouse-prevMouse;
        }
        else if ((!SelectedRec.Contains(currMouse) && MouseHelper.isPressed() && !MoveEveryting) || (RotateIt && !MoveEveryting))
        {
            RotateIt=true;
            Vector2 currWorld = currMouse;
            Vector2 prevWorld = prevMouse;

            Vector2 currDir = Vector2.Normalize(currWorld - SelectedRec.centrum);
            Vector2 prevDir = Vector2.Normalize(prevWorld - SelectedRec.centrum);

            float angle = (float)Math.Atan2(currDir.Y, currDir.X) - (float)Math.Atan2(prevDir.Y, prevDir.X);

            // Fix för hopp över PI-gränsen
            if (angle > Math.PI) angle -= MathHelper.TwoPi;
            if (angle < -Math.PI) angle += MathHelper.TwoPi;

            SelectedRec.rotation += angle;
        }
    }   
    protected override void DeleteInReSize()
    {
        if (KeyboardHelper.IsKeyDown(Keys.Delete)&&KeyboardHelper.AllKeysUp())
        {
            Lmin.Remove(SelectedRec);
            SelectedRec=null;
        }
    }
    protected override void MoveUpOrDownInList()
    {
        if (KeyboardHelper.AllKeysUp())
        {
            if (KeyboardHelper.IsKeyDown(Keys.Up))
            {
                int a = Lmin.IndexOf(SelectedRec);
                if(Lmin.Count>a+1){
                    MinRotRect after = Lmin[a+1];
                    Lmin[a]=after;
                    Lmin[a+1]=SelectedRec;
                }
            }
            if (KeyboardHelper.IsKeyDown(Keys.Down))
            {
                int a = Lmin.IndexOf(SelectedRec);
                if (a != 0)
                {
                    MinRotRect before = Lmin[a-1];
                    Lmin[a]=before;
                    Lmin[a-1]=SelectedRec;
                }
            }
        }
    }
    
    public override void Update()
    {
        if (KeyboardHelper.IsKeyDown(Keys.B))
        {
            Bstep = BuildOrOther.Build;
        }
        else if (KeyboardHelper.IsKeyDown(Keys.O))
        {
            Bstep=BuildOrOther.Other;
        }



        if(Bstep==BuildOrOther.Build){
            BuildNewRecs();
        }
        else if (Bstep == BuildOrOther.Other)
        {
            ReSizeMove();
        }
    }
    public override void Draw(SpriteBatch _spritebatch, Texture2D texture)
    {
                // för att visa om man bygger eller ändrar. det kan ändras från enum till att göras i samma men lite lättare att använda om man har det så.
        if (Bstep == BuildOrOther.Build)
        {
            _spritebatch.GraphicsDevice.Clear(Color.DarkGray);
        }
        else
        {
            _spritebatch.GraphicsDevice.Clear(Color.DarkBlue);
        }


         _spritebatch.Begin(transformMatrix:camera2D.get_transformation());
            
            foreach (MinRotRect rec in Lmin)
            {
                if(SelectedRec!=rec||Bstep==BuildOrOther.Build){
                    _spritebatch.Draw(texture,rec.centrum,null,new Color(rec.centrum_x/1800,0,rec.centrum_y/1000),rec.rotation,Vector2.One/2,new Vector2(rec.width / (float)texture.Width,rec.height / (float)texture.Height),SpriteEffects.None,0f);
                }
                else
                {
                    _spritebatch.Draw(texture,SelectedRec.centrum,null,Color.Red,SelectedRec.rotation,Vector2.One/2,new Vector2(SelectedRec.width / (float)texture.Width,SelectedRec.height / (float)texture.Height),SpriteEffects.None,0f);
                    //_spritebatch.Draw(texture,new MinRectangle(SelectedRec.GetSortedCorners()[0],10,10).rec,Color.Red);    
                //_spritebatch.Draw(texture,NewRotation.centrum,null,Color.DarkRed,NewRotation.rotation,Vector2.One/2,new Vector2(NewRotation.width / (float)texture.Width,NewRotation.height / (float)texture.Height),SpriteEffects.None,0f);
                }
            }
            _spritebatch.End();
    }
}
