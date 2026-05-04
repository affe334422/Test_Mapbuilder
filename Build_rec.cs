
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Build_rec
{
    /*
        contols,
        klicka bokstav O för att kunna välja att ändra storlek, position och rotation.
        högerklicka på den rektangel du vill ändra på efter du klickat o.
        klicka B för att bygga nya, klicka en gång för att starta, dra för att öka storlek
        och clicka igen för att sluta bygga.
        om du klickar delete i O kommer den du valt försvina och om du klickar B försviner den som är sist i listan försvina.
    */
    public Build_rec(){ // om du ska använda den för att bygga ny map. glöm inte lägga till den i mappen antingen med en spara fil som sparar position storlek och rotation.
        Lmin = new List<MinRotRect>();
    }
    public Build_rec(List<MinRotRect> Lmin) // om du redan har en map du vill ändra, samma som innan glöm inte spara.
    {
        this.Lmin=Lmin;
    }
    public List<MinRotRect> ListOfRec{get=>Lmin;} 
    /*
    behöver jag komma ihåg att göra så listan ligger i "rummet" 
    jag är i så jag inte har alla i samma lista.
    ja jag kan göra ett stort rum med camera2d.
    */
    private List<MinRotRect> Lmin;
    private enum BuildOrOther
    {
        Build,
        Other
    }
    private BuildOrOther Bstep; // bygger du eller ändrar du.


    public Vector2 StartPos;
    private bool IsItANewRecs = true;
    public void BuildNewRecs()// bygger nya recktanglar
    {
        BuildANewRec();
        DeleteInBuilder();
    }
    private void BuildANewRec()
    {
        if (IsItANewRecs&&MouseHelper.Click())
        {
            StartPos = MouseHelper.CurretPosition();
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
                Vector2 current = MouseHelper.CurretPosition();
                int x = (int)Math.Min(StartPos.X, current.X);
                int y = (int)Math.Min(StartPos.Y, current.Y);
                int width = (int)Math.Abs(current.X - StartPos.X);
                int height = (int)Math.Abs(current.Y - StartPos.Y);
                Lmin.Last().ChangeSize(width,height);
                Lmin.Last().centrum = new Vector2(x+width/2,y+height/2);
            }
        }
    }
    private void DeleteInBuilder()
    {
        if (KeyboardHelper.AllKeysUp()&&KeyboardHelper.IsKeyDown(Keys.Delete)&&Lmin.Count>0)
        {
            Lmin.Remove(Lmin.Last());
            IsItANewRecs=true;
        }
    }
    

    private MinRotRect SelectedRec;
    int corner;
    public void ReSizeMove() // Ändra storlek eller det du vill
    {
        ChooseARec();
        if (SelectedRec != null)
        {
            ReSizeOrMove();
            DeleteInReSize();
            MoveUpOrDownInList();
        }
    }
    private bool MoveEveryting = false;
    private bool RotateIt=false;
    private void ChooseARec()
    {
        foreach(MinRotRect min in Lmin)
        {
            if (min.Contains(MouseHelper.CurretPosition())&&MouseHelper.RightClick())
            {
                SelectedRec = min;
            }
        }   
    }
    private void ReSizeOrMove()
    {
        if(!MouseHelper.isPressed()){
            MoveEveryting=false;
            RotateIt=false;
            corner = SelectedRec.VilketHörnRörDu(MouseHelper.CurretPosition());
        }
        else if (corner != -1)
        {
            int opposite = (corner + 2) % 4;

            Vector2 fixedCorner = SelectedRec.hörn[opposite];

            Vector2 center = (fixedCorner + MouseHelper.CurretPosition()) / 2f;

            Vector2 axisX = new Vector2(
                (float)Math.Cos(SelectedRec.rotation),
                (float)Math.Sin(SelectedRec.rotation));

            Vector2 axisY = new Vector2(-axisX.Y, axisX.X);

            Vector2 diff = MouseHelper.CurretPosition() - fixedCorner;

            float width = Math.Abs(Vector2.Dot(diff, axisX));
            float height = Math.Abs(Vector2.Dot(diff, axisY));

            SelectedRec.centrum = center;
            SelectedRec.ChangeSize((int)(width+0.5), (int)(height+0.5));
        }
        else if (SelectedRec.Contains(MouseHelper.CurretPosition())&&MouseHelper.isPressed()&&!RotateIt||MoveEveryting&&!RotateIt)
        {
            MoveEveryting=true;
            SelectedRec.centrum+=MouseHelper.CurretPosition()-MouseHelper.PreviousPosition();
        }else if (!SelectedRec.Contains(MouseHelper.CurretPosition())&&MouseHelper.isPressed()&&!MoveEveryting||RotateIt&&!MoveEveryting)
        {
            RotateIt=true;
            Vector2 curr = MouseHelper.CurretPosition() - SelectedRec.centrum;
            Vector2 prev = MouseHelper.PreviousPosition() - SelectedRec.centrum;
            SelectedRec.rotation+=(float)Math.Atan2(curr.Y, curr.X)-(float)Math.Atan2(prev.Y, prev.X);
        }
    }   
    private void DeleteInReSize()
    {
        if (KeyboardHelper.IsKeyDown(Keys.Delete)&&KeyboardHelper.AllKeysUp())
        {
            Lmin.Remove(SelectedRec);
            SelectedRec=null;
        }
    }
    private void MoveUpOrDownInList()
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
    
    
    
    public void Update()
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

    public void Draw(SpriteBatch _spritebatch,Texture2D texture)
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

        _spritebatch.Begin();
        
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
