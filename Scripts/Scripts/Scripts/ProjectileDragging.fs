namespace MadAvians
open UnityEngine

type ProjectileDragging () =
    inherit MonoBehaviour ()

    (** Maximum Stretch *)
    [<SerializeField>]
    let mutable maxStretch = 3.0f

    (** Catapult Line Front *)
    [<DefaultValue>][<SerializeField>]
    val mutable catapultLineFront:LineRenderer
    
    //let mutable catapultLineFront = Unchecked.defaultof<LineRenderer>

    (** Catapult Line Back *)
    [<DefaultValue>][<SerializeField>]
    val mutable catapultLineBack:LineRenderer
    
    //let mutable catapultLineBack = Unchecked.defaultof<LineRenderer>

    [<DefaultValue>] val mutable private spring:SpringJoint2D
    [<DefaultValue>] val mutable private catapult:Transform
    [<DefaultValue>] val mutable private rayToMouse:Ray
    [<DefaultValue>] val mutable private leftCatapultToProjectile:Ray
    [<DefaultValue>] val mutable private prevVelocity:Vector2

    let mutable maxStretchSqr:float32 = 0.0f
    let mutable circleRadius:float32 = 0.0f
    let mutable clickedOn:bool = false
    
    (** *)
    member this.Awake () =
        this.spring <- this.GetComponent<SpringJoint2D>()
        this.catapult <- this.spring.connectedBody.transform
        
    (** *)
    member this.Start () =
        this.LineRendererSetup()
        this.rayToMouse <- new Ray(this.catapult.position, Vector3.zero)
        this.leftCatapultToProjectile <- new Ray(this.catapultLineFront.transform.position, Vector3.zero)
        maxStretchSqr <- maxStretch * maxStretch
        let circle = this.GetComponent<Collider2D>() :?> CircleCollider2D
        circleRadius <- circle.radius
                        

    (** *)
    member this.Update () =
        if clickedOn = true then
            this.Dragging()
           
        if this.spring <> null then
            if not(this.GetComponent<Rigidbody2D>().isKinematic)
             && this.prevVelocity.sqrMagnitude > this.GetComponent<Rigidbody2D>().velocity.sqrMagnitude
              then
                Object.Destroy(this.spring)
                this.GetComponent<Rigidbody2D>().velocity <- this.prevVelocity
                
             
            if not clickedOn then
                this.prevVelocity <- this.GetComponent<Rigidbody2D>().velocity
                
            this.LineRendererUpdate()
            
        else
            this.catapultLineFront.enabled <- false
            this.catapultLineBack.enabled <- false
              
    (** *)
    member this.LineRendererSetup () =  
        this.catapultLineFront.SetPosition(0, this.catapultLineFront.transform.position)
        this.catapultLineBack.SetPosition(0, this.catapultLineBack.transform.position)
        
        this.catapultLineFront.sortingLayerName <- "Foreground"
        this.catapultLineBack.sortingLayerName <- "Foreground"
        
        this.catapultLineFront.sortingOrder <- 3
        this.catapultLineBack.sortingOrder <- 1
        
        
    (** *)
    member this.OnMouseDown () = 
        this.spring.enabled <- false
        clickedOn <- true
        
        
    (** *)
    member this.OnMouseUp () = 
        this.spring.enabled <- true
        this.GetComponent<Rigidbody2D>().isKinematic <- false
        clickedOn <- false
        
        
    (** *)
    member this.Dragging() =
        let mutable mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition)
        let catapultToMouse = mouseWorldPoint - this.catapult.position
        
        if catapultToMouse.sqrMagnitude > maxStretchSqr then
            this.rayToMouse.direction <- catapultToMouse
            mouseWorldPoint <- this.rayToMouse.GetPoint(maxStretch)
        
        mouseWorldPoint.z <- 0.0f
        this.transform.position <- mouseWorldPoint
        
        
    (** *)
    member this.LineRendererUpdate() =
        let mutable catapultToProjectile = this.transform.position - this.catapultLineFront.transform.position
        this.leftCatapultToProjectile.direction <- catapultToProjectile
        let mutable holdPoint = this.leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius)
        this.catapultLineFront.SetPosition(1, holdPoint)
        this.catapultLineBack.SetPosition(1, holdPoint)
        
        