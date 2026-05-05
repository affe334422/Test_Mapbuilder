
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public abstract class _GameRunSetup
{
    protected KeyboardState kstate;
    protected GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected Texture2D texture;
    protected Camera2D camera2D;
    public _GameRunSetup(Camera2D camera2D, GraphicsDeviceManager _graphics, SpriteBatch _spriteBatch, Texture2D texture)
    {
        this.camera2D = camera2D;
        this._graphics = _graphics;
        this._spriteBatch = _spriteBatch;
        this.texture = texture;
    }
    public abstract void Update(GameTime gameTime);
    public abstract void Draw();
}
