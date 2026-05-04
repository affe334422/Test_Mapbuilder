
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class LitenStudsBoll : MinRotRect
{
    public LitenStudsBoll(Vector2 xy, int width, int height) : base(xy,width,height)
    {
        Velocity = new Vector2(SmåHjälpmedel.Ran.Next(-6,6),SmåHjälpmedel.Ran.Next(-6,6));
    }
    private Vector2 Velocity;
    private bool HasColided = true;
    public void Update(List<MinRotRect> ListMedRec)
    {
        HasColided = false;
        Colicions(ListMedRec);
        Move();
    }
    private void Move()
    {
        Centrum+=Velocity;
        Velocity.Y+=0.3f;
        Velocity*=0.997f;
    }


    Vector2 GetNormal(MinRotRect r, Vector2 point)
    {
        Vector2 bestNormal = Vector2.Zero;
        float bestDist = float.MaxValue;

        for (int i = 0; i < 4; i++)
        {
            Vector2 a = r.hörn[i];
            Vector2 b = r.hörn[(i + 1) % 4];

            Vector2 edge = b - a;
            Vector2 normal = new Vector2(-edge.Y, edge.X);
            normal.Normalize();

            float dist = Vector2.Dot(point - a, normal);

            if (Math.Abs(dist) < bestDist)
            {
                bestDist = Math.Abs(dist);
                bestNormal = normal;
            }
        }

        return bestNormal;
    }
    public void Colicions(List<MinRotRect> ListMedRec)
    {
        if (HasColided) return;

        foreach (MinRotRect interact in ListMedRec)
        {
            if (interact.Contains(centrum))
            {
                Vector2 normal = GetNormal(interact, centrum);
                
                if (Vector2.Dot(Velocity, normal) > 0)
                {
                    //normal = -normal;
                }

                
                //while (interact.Contains(centrum)||true)
                //{
                  //  centrum += normal;
                //}
                centrum-=Velocity;
                Velocity = Velocity - 2 * Vector2.Dot(Velocity, normal) * normal;
                centrum+=Velocity;
                Velocity*=0.8f;

                HasColided = true;
                break;
            }
        }
    }
}
