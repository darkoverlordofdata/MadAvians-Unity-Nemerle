namespace MadAvians
open UnityEngine

(** ProjectileFollow

    Make the camera follow the projectile
*)
type ProjectileFollow () =
    inherit MonoBehaviour ()

    (** The transform of the projectile to follow. *)
    [<SerializeField>]
    let mutable projectile = Unchecked.defaultof<Transform>

    (** The transform representing the left bound of the camera's position. *)
    [<SerializeField>]
    let mutable farLeft = Unchecked.defaultof<Transform>

    (** The transform representing the right bound of the camera's position. *)
    [<SerializeField>]
    let mutable farRight = Unchecked.defaultof<Transform>

    member this.Update () =
        (* Store the position of the camera. *)
        let mutable newPosition = this.transform.position

        (* Set the x value of the stored position to that of the bird. *)
        newPosition.x <- projectile.position.x
        
        (* Clamp the x value of the stored position between the left and right bounds. *)
        newPosition.x <- Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x)
        
        (* Set the camera's position to this stored position. *)
        this.transform.position <- newPosition;