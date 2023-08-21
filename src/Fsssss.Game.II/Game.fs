namespace Fsssss.Game.II
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type FsssssGame() as this = 
    inherit Game()

    let background = Color.DarkGreen
    let graphics = new GraphicsDeviceManager(this)
    let mutable state = Unchecked.defaultof<GameState>
    let mutable pixel = Unchecked.defaultof<Texture2D>

    do
        this.Content.RootDirectory <- "Content"
        graphics.PreferredBackBufferWidth <- 300
        graphics.PreferredBackBufferHeight <- 300
        graphics.GraphicsProfile <- GraphicsProfile.HiDef
        graphics.IsFullScreen <- false

    member this.Graphics = graphics

    override this.Initialize() =
        base.Initialize()
        state <- State.create() |> State.init
    
    override this.LoadContent() =
        pixel <- new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color)
        pixel.SetData([| Color.White |])
        base.LoadContent()

    override this.Update(gametime) =
        base.Update(gametime)

    override this.Draw(gametime) =
        let fps = 1.0 / gametime.ElapsedGameTime.TotalSeconds
        this.Window.Title <- sprintf "Fsssss.Game.II - %ffps" fps
        clearScreen this.GraphicsDevice background
        State.draw this.GraphicsDevice pixel state
        base.Draw(gametime)