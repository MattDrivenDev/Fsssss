namespace Fsssss.Game.II

type Tile = | Empty | Wall | Food | Snake

type GameState = {
    Map : Tile array2d 
}