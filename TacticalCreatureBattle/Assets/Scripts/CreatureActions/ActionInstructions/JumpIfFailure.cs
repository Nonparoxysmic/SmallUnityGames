using System.Collections;

public class JumpIfFailure : ActionInstruction
{
    public int JumpValue;
    public bool ValueIsAbsolute;

    public override IEnumerator Execute()
    {
        if (!Action.InstructionSuccess)
        {
            DoJump();
        }
        yield break;
    }

    void DoJump()
    {
        if (ValueIsAbsolute)
        {
            if (Action.CurrentInstruction == JumpValue)
            {
                // Do not jump to the current instruction.
                return;
            }
            Action.CurrentInstruction = JumpValue - 1;
            return;
        }
        if (JumpValue == 0)
        {
            // Do not jump to the current instruction.
            return;
        }
        Action.CurrentInstruction += JumpValue - 1;
    }
}
