namespace MadAvians
open UnityEngine
open System.Collections;

(** Resetter

    Reset the game when bounds are exceeded
*)
type Resetter () =
    inherit MonoBehaviour ()

    (** The rigidbody of the projectile *)
    [<SerializeField>]
    let mutable projectile = Unchecked.defaultof<Rigidbody2D>

    (** The angular velocity threshold of the projectile, below which our game will reset *)
    [<SerializeField>]
    let mutable resetSpeed = 0.025f
    
    (** The square value of Reset Speed, for efficient calculation *)
    [<DefaultValue>] val mutable private resetSpeedSqr:float32
    
    (** The SpringJoint2D component which is destroyed when the projectile is launched *)
    [<DefaultValue>] val mutable private spring:SpringJoint2D

    (** Start *)
    member this.Start () =
        (* Calculate the Resset Speed Squared from the Reset Speed *)
        this.resetSpeedSqr <- resetSpeed * resetSpeed

        (* Get the SpringJoint2D component through our reference to the GameObject's Rigidbody *)
        this.spring <- projectile.GetComponent<SpringJoint2D>()
    
    (** Update *)
    member this.Update () =
        (* Store the position of the camera. *)
        if Input.GetKeyDown (KeyCode.R) then
            (* ... call the Reset() function *)
            this.Reset()
                        
        (* If the spring had been destroyed (indicating we have launched the projectile) and our projectile's velocity is below the threshold... *) 
        if this.spring = null && projectile.velocity.sqrMagnitude < this.resetSpeedSqr then
            (* ... call the Reset() function *)
            this.Reset()
        
    (** OnTriggerExit2D
     * 
     * @param other - the other game object we collided with
     *)
    member this.OnTriggerExit2D (other:Collider2D) =
        (* If the projectile leaves the Collider2D boundary... *)
        if other.GetComponent<Rigidbody2D>() = projectile then
            (* ... call the Reset() function *)
            this.Reset()

    (** Reset *)
    member this.Reset () = 
        (* The reset function will Reset the game by reloading the same level *)
        Application.LoadLevel(Application.loadedLevel)