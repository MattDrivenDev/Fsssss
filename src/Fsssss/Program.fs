namespace Fsssss

module Program = 

    [<EntryPoint>]
    let main argv = 
        let game = new FsssssGame()
        game.Run()

        0 // return an integer exit code