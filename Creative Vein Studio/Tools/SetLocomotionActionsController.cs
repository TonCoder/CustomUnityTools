using UnityEngine;

public static class SetLocomotionActionsController 
{
#region Interactions

    public static bool GetTurnAngles(ref Transform trans, Vector3 direction, out float currentAngle, out float targetAngle)
    {
        if (direction == Vector3.zero)
        {
            currentAngle = float.NaN;
            targetAngle = float.NaN;
            return false;
        }

        currentAngle = trans.eulerAngles.y;
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return true;
    }

    public static Vector3 TurnTowardsAngle(float currentAngle, float targetAngle, float speed)
    {
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed * Time.deltaTime);
        return new Vector3(0, currentAngle, 0);
    }

    public static void MoveByPos(Vector3 moveDir, float moveSpeed, Rigidbody rigidB)
    {
        var move = rigidB.position + new Vector3(moveDir.x, 0, 0) * Time.deltaTime * moveSpeed;
        rigidB.MovePosition(move);
    }

    public static void MoveByVelocity(Vector3 moveDir, float moveSpeed, Rigidbody rigidB)
    {
        var moveToPos = moveDir * moveSpeed * Time.deltaTime;
        rigidB.velocity = moveToPos;
    }
    
    public static void TurnLeftRight2D(Vector3 moveDir, Rigidbody rigidB){
        if(moveDir == Vector3.zero) return;
        Quaternion rotateTo = Quaternion.Euler(new Vector3(0, moveDir.x > 0? 90 : -90, 0));
        rigidB.rotation = rotateTo;
    }
   
    public static void TurnWithLerp(Vector3 moveDir, float turnSpeed, Rigidbody rigidB){
        if(moveDir == Vector3.zero) return;
        Quaternion rotateTo = Quaternion.Euler(new Vector3(0, moveDir.x > 0? 90 : -90, 0));
        var lerpTo = Quaternion.Lerp(rigidB.rotation, rotateTo, turnSpeed);
        // Creature.TurnKillable(lerpTo);
        rigidB.rotation = lerpTo;
    }

    public static void Jump(float jumpForce, Rigidbody rigidB)
    {
        rigidB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public static void JumpForward(float jumpForce, Vector3 dir,  Rigidbody rigidB = null)
    {
        dir.y = 0;
        rigidB.AddForce((dir + Vector3.up) * jumpForce, ForceMode.Impulse);
    }

    public static void AddGravity(Vector3 gravityDir,  float gravity, float gravityMultiplier, Rigidbody rigidB)
    {
        Vector3 val = gravityDir * -Mathf.Abs(gravity) *  gravityMultiplier;
        rigidB.AddForce(val, ForceMode.Force);    
    }
#endregion
}
