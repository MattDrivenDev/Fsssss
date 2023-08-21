namespace Fsssss.Game.II

type Tile = | Empty | Wall | Food | Snake

type Movement = | None | Up | Down | Left | Right

type GameState = {
    Map : Tile array2d 
}