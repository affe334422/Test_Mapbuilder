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
    public Vector2[] GetSortedCorners()
    {
        Vector2[] sorted = (Vector2[])Hörn.Clone();

        Array.Sort(sorted, (a, b) =>
        {
            if (a.Y == b.Y)
                return a.X.CompareTo(b.X); // vänster först
            return a.Y.CompareTo(b.Y);     // överst först
        });

        return sorted;
    }
    public bool Contains(Vector2 point)
    {
        // Flytta till lokal space
        Vector2 local = point - Centrum;

        // Rotera tillbaka (inverse rotation)
        float cos = (float)Math.Cos(-Rotation);
        float sin = (float)Math.Sin(-Rotation);

        float x = local.X * cos - local.Y * sin;
        float y = local.X * sin + local.Y * cos;

        // Kolla inom box
        return Math.Abs(x) <= Width / 2f &&
            Math.Abs(y) <= Height / 2f;
    }
}
