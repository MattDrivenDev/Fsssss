namespace Fsssss.Game
open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

[<AbstractClass>]
/// A wrapper for the standard XNA Game class - I wanted to wrap a few of the
/// members of the class that allowed a concrete implementation to be more 
/// functional and declarative. It helps hide away some of the noise as well.
type FsGame(contentRoot, configureGraphics) as this = 
    inherit Game()    
      
    let random = new Random()
    let graphics = new GraphicsDeviceManager(this)
    let mutable spritebatch = Unchecked.defaultof<_>
    
    do  this.Content.RootDirectory <- contentRoot
        configureGraphics graphics

    /// A list of functions to run when the game is initialized.
    abstract InitializeSteps : (unit -> unit) list

    /// A list of functions to run when the game loads content.
    abstract LoadContentSteps : (unit -> unit) list

    /// A list of functions to run during the Update part of the game loop.
    abstract UpdateSteps : (GameTime -> unit) list

    /// A list of functions to run during the Draw part of the game loop.
    abstract RenderSteps : (SpriteBatch -> GameTime -> unit) list
    
    member this.Graphics = graphics

    member this.Random = random
    
    override this.Initialize() = 
        base.Initialize()
        this.InitializeSteps |> List.iter(fun f -> f())
        
    override this.LoadContent() =         
        spritebatch <- new SpriteBatch(graphics.GraphicsDevice)        
        this.LoadContentSteps |> List.iter(fun f -> f())

    override this.Update(gametime) =         
        this.UpdateSteps |> List.iter(fun f -> f gametime)

    override this.Draw(gametime) = 
        this.RenderSteps |> List.iter(fun f -> f spritebatch gametime)