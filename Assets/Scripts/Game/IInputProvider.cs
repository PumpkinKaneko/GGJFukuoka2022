public interface IInputProvider
{
    public bool GetInputFront();

    public bool GetInputBack();

    public bool GetInputRight();

    public bool GetInputLeft();

    public bool GetInputActionA();
    
    public bool GetInputActionB();
    
    public bool GetInputActionC();

    public float GetAxisX();
    
    public float GetAxisY();

    
}
