You need to create a function in your playermovement script that is :

    [HideInInspector] public bool isTouching;

    private void OnCollisionExit(Collision collision)
    {
        isTouching = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isTouching = true;
    }
    
    So your Smooth Landing works
    
