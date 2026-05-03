using System;
using Microsoft.Xna.Framework;
public class MinRotRect
{
    protected Vector2 Centrum;
    protected float Rotation; // Radianer
    protected int Width;
    protected int Height;
    protected Vector2[] Hörn = new Vector2[4];

    public MinRotRect(int x, int y, int width, int height)
    {
        Centrum = new Vector2(x, y);
        Width = width;
        Height = height;
        Rotation = 0f;
        UppdateraHörn();
    }
    public MinRotRect(Vector2 xy, int width, int height)
    {
        Centrum = xy;
        Width = width;
        Height = height;
        Rotation = 0f;
        UppdateraHörn();
    }
    public MinRotRect(float Rotation, Vector2 xy, int width, int height)
    {
        Centrum = xy;
        Width = width;
        Height = height;
        this.Rotation = Rotation;
        UppdateraHörn();
    }
    public int width{get=>Width;}
    public int height{get=>Height;}
    public Rectangle rec{get=>new Rectangle((int)centrum.X-Width/2,(int)centrum.Y-Height/2,Width,Height);}
    public float centrum_x
    {
        get => Centrum.X;
        set
        {
            Centrum.X = value;
            UppdateraHörn();
        }
    }
    public float centrum_y
    {
        get => Centrum.Y;
        set
        {
            Centrum.Y = value;
            UppdateraHörn();
        }
    }
    public Vector2 centrum
    {
        get => Centrum;
        set
        {
            Centrum = value;
            UppdateraHörn();
        }
    }
    public float rotation
    {
        get => Rotation;
        set
        {
            Rotation = value;
            UppdateraHörn();
        }
    }
    public Vector2[] hörn{get=> Hörn;}
    public void ChangeSize(int width, int height)
    {
        Width = width;
        Height = height;
        UppdateraHörn();
    }
    public int VilketHörnRörDu(Vector2 mouse, float radius = 20f)
    {
        for (int i = 0; i < Hörn.Length; i++)
        {
            if (Vector2.Distance(mouse, Hörn[i]) <= radius)
                return i;
        }
        return -1;
    }
    private void UppdateraHörn()
    {
        float hw = Width / 2f;
        float hh = Height / 2f;

        Vector2[] lokala = new Vector2[4]
        {
            new Vector2(-hw, -hh),
            new Vector2(hw, -hh),
            new Vector2(hw, hh),
            new Vector2(-hw, hh)
        };

        for(int i=0;i<4;i++)
        {
            float x = lokala[i].X;
            float y = lokala[i].Y;

            float rx =
                x * (float)Math.Cos(Rotation)
                - y * (float)Math.Sin(Rotation);

            float ry =
                x * (float)Math.Sin(Rotation)
                + y * (float)Math.Cos(Rotation);

            Hörn[i] = new Vector2(
                Centrum.X + rx,
                Centrum.Y + ry);
        }
    }
    public bool Contains(Vector2 punkt)
    {
        bool inside=false;

        for(int i=0;i<Hörn.Length;i++)
        {
            int j=(i+1)%Hörn.Length;

            Vector2 A=Hörn[i];
            Vector2 B=Hörn[j];

            bool intersect =
                ((A.Y>punkt.Y)!=(B.Y>punkt.Y)) &&
                (punkt.X<
                (B.X-A.X)*
                (punkt.Y-A.Y)/
                (B.Y-A.Y)
                +A.X);

            if(intersect)
                inside=!inside;
        }

        return inside;
    }
}
