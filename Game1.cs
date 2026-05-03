using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D texture;
    _GameRunSetup _GameRun;
    SpriteFont GameFont;
    bool start = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight=1000;
        _graphics.PreferredBackBufferWidth=1800;
    }
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here  
        base.Initialize();
    }
    protected override void LoadContent()
    {
        GameFont = Content.Load<SpriteFont>("Gamefont");
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        texture = new Texture2D(GraphicsDevice, 1, 1);
        texture.SetData(new[] {Color.White});
        // TODO: use this.Content to load your game content here
    }
    protected override void Update(GameTime gameTime)
    {
        MouseHelper.Update();
        KeyboardHelper.Update();
        if (start)
        {
            start=false;
            _GameRun = new Platform_builderV1(_graphics,_spriteBatch,texture);
        }
        _GameRun.Update(gameTime);

        
        if (KeyboardHelper.IsKeyDown(Keys.Escape)){
            Exit();
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _GameRun.Draw();
        base.Draw(gameTime);
    }
}
