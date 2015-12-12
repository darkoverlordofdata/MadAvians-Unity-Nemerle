namespace MadAvians
 
open UnityEngine
 
type SimpleScript() = 
    inherit MonoBehaviour()
 
    // Inspector pane property
    [<SerializeField>]
    let mutable numCubes = 3
 
    // Use the Unity3D API
    let spawnCube x =
        let cube = GameObject.CreatePrimitive(PrimitiveType.Cube)
        cube.transform.position <- new Vector3(x, 0.0f, 0.0f)
 
    member this.Start() =
        // Spawn the number of cubes specified in the Inspector pane
        let zx = 1.0f
        let zy = 2.0f
        let s = sprintf "this is a test: %f %f" zx zy
        [1..numCubes] |> List.map (fun x -> float32 x * 1.5f) |> List.iter spawnCube