
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Camera2D
{
    protected float _zoom; // Camera Zoom
    protected Matrix _transform; // Matrix Transform
    protected Vector2 _pos; // Camera Position
    protected float _rotation; // Camera Rotation
    private GraphicsDevice graphicsDevice;

    public Camera2D(GraphicsDevice graphicsDevice)
    {
        _zoom = 1f;
        _rotation = 0.0f;
        _pos = new Vector2(900,500);
        this.graphicsDevice = graphicsDevice;
    }
    public float Zoom
    {
        get { return _zoom; }
        set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
    }

    public float Rotation
    {
        get {return _rotation; }
        set { _rotation = value; }
    }

    // Auxiliary function to move the camera
    protected Vector2 Vel = new Vector2(0,0);
    protected float ZoomSpeed = 0;
    protected float RotateSpeed = 0;
    protected float Friction = 0.9f;
    protected int Max = 100;
    public void Move()
    {
        //side to side
        if (KeyboardHelper.IsKeyUp(Keys.A) && KeyboardHelper.IsKeyUp(Keys.D)||KeyboardHelper.IsKeyDown(Keys.A)&&KeyboardHelper.IsKeyDown(Keys.D)){ 
            Vel.X *= Friction;
            if (Vel.X < 1 && -1 < Vel.X)
            {
                Vel.X = 0;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.A))
        {
            Vel.X -= 1;
            if(Vel.X>0){
                Vel.X*=Friction;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.D))
        {
            Vel.X += 1;
            if(Vel.X<0){
                Vel.X*=Friction;
            }
        }
        //up and down
        if (KeyboardHelper.IsKeyUp(Keys.W) && KeyboardHelper.IsKeyUp(Keys.S)||KeyboardHelper.IsKeyDown(Keys.W) && KeyboardHelper.IsKeyDown(Keys.S)){ 
            Vel.Y *= Friction;
            if (Vel.Y < 1 && -1 < Vel.Y)
            {
                Vel.Y = 0;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.W))
        {
            Vel.Y -= 1;
            if(Vel.Y>0){
                Vel.Y*=Friction;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.S))
        {
            Vel.Y += 1;
            if(Vel.Y<0){
                Vel.Y*=Friction;
            }
        }
        double V = Math.Atan2(Vel.Y,Vel.X);
        double Hyp = Math.Pow(Math.Pow(Vel.X,2)+Math.Pow(Vel.Y,2),0.5);
        if(Hyp > Max){
            Vel.X = (float)(Max *Math.Cos(V));
            Vel.Y = (float)(Max *Math.Sin(V));
        }
        Pos += Vel;
        Rotate_And_Zoom();
    } 
    private void Rotate_And_Zoom()
    {
        //rotate
        if (KeyboardHelper.IsKeyUp(Keys.Left) && KeyboardHelper.IsKeyUp(Keys.Right)||KeyboardHelper.IsKeyDown(Keys.Left)&&KeyboardHelper.IsKeyDown(Keys.Right)){ 
            RotateSpeed *=Friction;
            if (RotateSpeed < 1 && -1 < RotateSpeed)
            {
                RotateSpeed = 0;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.Left))
        {
            RotateSpeed -= 1;
            if(RotateSpeed>0){
                RotateSpeed*=Friction;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.Right))
        {
            RotateSpeed += 1;
            if(RotateSpeed<0){
                RotateSpeed*=Friction;
            }
        }
        Rotation+=RotateSpeed/500;
        //zoom
        if (KeyboardHelper.IsKeyUp(Keys.Down) && KeyboardHelper.IsKeyUp(Keys.Up)||KeyboardHelper.IsKeyDown(Keys.Down) && KeyboardHelper.IsKeyDown(Keys.Up)){ 
            ZoomSpeed *= Friction;
            if (ZoomSpeed < 1 && -1 < ZoomSpeed)
            {
                ZoomSpeed = 0;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.Down))
        {
            ZoomSpeed -= 1;
            if(ZoomSpeed>0){
                ZoomSpeed*=Friction;
            }
        }
        else if (KeyboardHelper.IsKeyDown(Keys.Up))
        {
            ZoomSpeed += 1;
            if(ZoomSpeed<0){
                ZoomSpeed*=Friction;
            }
        }
        Zoom+=ZoomSpeed/500;
    }
    // Get set position
    public Vector2 Pos
    {
        get { return _pos; }
        set { _pos = value; }
    }
    public Matrix get_transformation()
    {
        _transform =       // Thanks to o KB o for this solution
            Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                        Matrix.CreateRotationZ(Rotation) *
                                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                        Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        return _transform;
    }
    public Vector2 get_mouse_pos_with_cam(Vector2 screenPos)
    {
        return Vector2.Transform(screenPos, Matrix.Invert(get_transformation()));
    }
}
