namespace Fsssss.Game.II
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type FsssssGame() = 
    inherit Game()

    let background = Color.Gray
    let mutable state = State.create()
    let mutable pixel = Unchecked.defaultof<_>

    override this.Initialize() =
        base.Initialize()
    
    override this.LoadContent() =
        pixel <- new Texture2D(base.GraphicsDevice, 1, 1, false, SurfaceFormat.Color)
        pixel.SetData([| Color.White |])
        base.LoadContent()

    override this.Update(gametime) =
        base.Update(gametime)

    override this.Draw(gametime) =
        clearScreen base.GraphicsDevice background
        State.draw base.GraphicsDevice pixel state
        base.Draw(gametime)