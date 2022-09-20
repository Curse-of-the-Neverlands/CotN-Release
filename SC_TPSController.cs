using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_TPSController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
}


/*
&&&&&&&&&@@@@@@@@@&&&&@@@@@@&&&&@@@@&&&@@@@&&&&&&&@@&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
&&&&&@@@@@@@@@@@@@&&@@@@@@@@@&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
&@@@@@@@@@@@@@@@@@@@@@@&&&&&&&&&&&&&&#&&&&#####&&&#&&&&&&&&&&&@@&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@&&&&&&&&&&&&&&&&&&&&##&&##BB###&&&&&&&&&&&&&&&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@&&@@@@&&&&&&&&&&&&&&&&&&&&#&#BGG&&###BBB#&&&##&####&&&&&&&&@@@&@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@&@@@@&&&#&&&&&&&&&&&&&&&&#BGB#BB#&&&&#####&#########&&&&&&&&&&@@@@@@&&@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@&&&###&&&&&#B#BBB###BBGBB##&##&&&&##B####B###########&&####&&&@@@@@&@@@@@@@@@@@@@@@@@@@
@@@@@@@@@&&&&&&&&###&&##BBBBBBGGBB##&###&##&###############&&&&&&####&####&&&@@@&@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@&&&&&&&#BBBBBBGB########&&##&&#######&&#######&&&&#####&#####&#B##&&&&@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@&&&&&&&&##BBBBB##&&&#####BBBBBBBBBBBBBGGPGGGBBBBBB####&&&&###&&&###&&@&&@@@@@@@@@@@@@@@@@@
@@@@@@@@@@&&&&&&&###BB##&#####BBGGPP555555YYYYYYYYY5555PPPPGGB##&&&&#BBB#&&&&#&&@@@@@@@@@@@@@@@@@@@@
@@@@@@@@&&&&&&&#####BBBBGGPP555YYYYJYYYJJJJJJJJJYYYYY55555555PGGBB#########&&&##&@@@@@@@@@@@@@@@@@@@
@@@@@@&&&&&&&&####BBGPPP555YYYYJJJJJJJJJJJJJJJJJJJJYYYYYY55555PPGB##########&&##&&@@@@@@@@@@@@@@@@@@
@@@@&&&&&&&&&###BGGPPPP555YYYYYJJJJJJJJJJJJJJ???JJJJJJYYYYY555PPGGBBBBBBB#&&&&###&&&&&@@@@@@@@@@@@@@
@@@@&&&&&@&&&&#GGPPPPPP5YYYJJJJJJJJJJJ?????????????JJJJYJYYYY555PPGBBBBB##&&&&###&&&@@@@@@@@@@@@@@@@
&&&&&&&&&&&&&#GPPGPPP55YYJJJJJJ?????J??????????????JJJJJJJJYYYY555PGGGGBB########&@&@@@@@@@@@@@@@@@@
&&&&&&@@@&&&&BGGPPP55YYYJJJJ??????JJJJJJJJJJ????JJJJJJJJJJJJJYYYY55PGGBB####BBB##&&&@@@@@@@@@@@@@@@@
@@@&&&&&&@&&#BGGP5555YYJJJJJJJ??JJJJJJJJJJJJJJJJJJJJYYYJJJJJJJJYYYY5PPGB#########&&&&&@@@@@@@@@@@@@@
@@@&&&&&&&&&#BGGPP55YYYJJJJJJJJJJJJJJJ????JJJJ?JJJJJJJYYYYYYYYJJJYYY55PGBB#BBBBB#&&&@@@@@@@@@@@@@@@@
@@@&&&&&&&&&&BGGP55YYYYYYYYYYJJJ?????????77?????????JJY55P55P55YYYYY555GB##BBBBBB&&&@@@@&&@@@@@@@@@@
@@@&&&&&&&&&&BGPP55YYYYYYJJJJ???????77777777777??JY55555YYYYY55PP5YY5555G###BBBBBB&&@@@@&@@@@@@@@@@@
@&&&&&&&&&&&&#GPP5P55YYJJJJJ????????777!!77?JJY5PPPPYYYYJJJ??JJY5555Y555PB######BB#&@&&@@&@&&&&&&&&&
&&&&&&&&&&&&&#GGGGGPPPPPPPPP5YJJ?????7777??JYY5555JJJJJJYYYJJ???JY555555PB&#B##BBBPYP#&&&&&&&&&&&&&&
&&&&&&&&&&&&&#BBBBGPPPPPGPPPPP5YYJ???777???JJJJJJYYY55Y5P555YJ???JYY5555PB###BGGP55??JG&&&&&&&&&&&&&
&&&&&&&&&@&&&##BG5YYYYYYYJJYYYYY55YJ?????JJJJJJJYYJ?PPPGGYYPYYJJJJJJYY55PGBBG5YY?7?JY?JG&&&&&&&&&&&&
&&&&&&&&&&&&&##BPYYYY55555555YJYY5Y5YJJJJYJJJJJYYJ??Y5555YY5YYYJJJJJJYY55PGG5YJJ??7JYJ?5#&#&&&&&&&&&
&&&&&&&&&&&&&&#GPYYY5PPY5GPGG5???Y555YJJYYYYYYYYYYYYJJJJJYYYYJJJYJJJJYY55PPP55JJ??77?JJ5B#&&&&&&&&&&
&&&&&&&&&&&&&##BGYY5GG5JJPGP5YJJY5P5YYJJYYYYYYYYYYYYYJJJJYYYJJJJJJJJJJYY55PP5YJ??77!7JJP##&#&&&&&&&&
&&&&&&&&&@&@BPBBG5Y5P555YJYYJYY555P5YYYYYYYYYYJJJJJJJJJJJJJJJJ?????JJJYYY5PPY????7???JYB&######&&&&&
&&&&&&&&&@&@GJYGP55555YYYYYYYYYY55G5JJJJYYYYJJJJ????????????????????JJYYY5P5J77?JJJJJJP#&#&#####&#&&
&&&&&&&&&&&@BYYPG555YYYYYYYJJYYY5P5YJJJJYYY5YYJ??J???????777???????JJJYY55PPJ!?JJJJYJYG###&#########
&&&&&&&&&@&@#PY5P55YYYYYYJJJYJJY55JJJ?7?JJJYYYYY?77777?77777777????JJJYY5555?777??JJYP##############
&&&&&&&&&&&@&B5?Y55YYJJJJJJJJJJYPY?777!!7??????YJ77!77777777777????JJJYY55PY77?JJ?JYG###############
&&&&&&&&&&&&&@BJJP5YYJJJJJJJ??JYYYJ??77?JY5P5Y??J??!!777777777??????JJY555GJ???JY5G#&&###&&&&&&&&&##
&&&&&&&&&&&&@@&PJ555YJJJJJJ??7??JJJJYJJJJJ?JYJ?JJ?7777777777???????JJJYY5PPYY5Y?JPGGGGGGGGGGGGGGGPPP
&&&&&&&&&@&&&&&&G5555YYJJJ??77??JJJJYJ????7??JJJJ??????7?77????????JJJYY55P55PG5YYYYYYYYYYY5YYYY5555
&&&&&&&&&@&&&&&@#55555YJJJJ????JJYY55YJ?JJJJJJJJJJJJJJ????????????JJJJJYY5PPP555Y5PPGGGGBBBGP55P55YY
&&&&&&&&&&@&&@&&&GPPP5YYYJJJJJYYYYY5YYJJJJJJJJJJJJJJJJJJJJ????????JJJJJYY5PP5JJJYYY555555555YY55YYYY
&&&&&&&&&&@&@@&&&#BGP5Y5YJJYYY55YYYYYJJJJJ???????????????JJ????????JJJJJY5GPYJJYJ?JYYYYYYYYYYYYYJJYY
&&&&&&&&@&@&&@&&#GPPPP55YYY555YYJJJYYYYJJJJJJJJJJJ????????J???JJJ??JJJJJY5P5YYJJJJYYYJJYJJYYYYYJJJYY
&&&&&&&&&&&&&@&#B5Y5PP5555555YJYYYY55555555555555555YYJ????????JJ??JJJJYY5PYJJJJJJYJ?JJYYJJYYYJJJYYY
&&&&&&&&@@&&@@#P5YYJYPP55555YJJYPPP55555YYYYYYYYYYYYJJ????????JJJ??JJJJY5PPYJJJJJYJ??JJJJYYYYJJ?JYYJ
&&&&&@@@&@&&&BP5YYYJJY5P5555YYY5P555YYYYYJJJ?JJJJJ????????????JJJJ?JJYY55P5YJ??JJYJ?JJJJJYYYYJ??JJJJ
&&&&&@@&&&@@BP5YYYYJ?JY5P555YYYYYJJYYYYY5Y55YYYJJJ?????????JJJJJ?JJYYY555Y?J???JJJ??JJJJJYYJJ??JJJ?J
&&&&&@@&&&@#P5YYYJJJJJJY5P555YYYYJJJYY5PP5PPP5YYJ??7??????JJJJJJJYYYJJY55??????J????JJJ?JYYJJ???JJJJ
&&&&&&@@&&#P5YYYJJJJJJ?JY5PP55YYYYJJJY5PP55555YYJ?777????JJJJJYYYJJJJY555?J???????7?JJ??JYYJ?7?JJJJY
&&&&&&&&@&G55YYYJJJJJJJJJYY5555YYJJJJY5PPYJYY55YJJ???????JJYYYYY??JJJY55Y?J??????77?JJ??JJJ?77JJJJYY
&&&&&@&&@GP55YYYYJJJJJJJYYYYY5P5YYYYY5555YYYJYYJJJJJJJYYYY55YJ???JJJJY555J???????77?J?????J???JJJJJJ
&&&&&&@@&P555YYJJJJJJJJJJJJJJY5PP555555555555YY55YJJY55555YJ?????JJJJY5P5J??????7!7????JJJJJJJJJJJJJ
&@&&@&&@BP555YYJJJJJJJJJJJJJJJY5BGGGPPP5P5555555PPPP5555YJ????????JJYY5P5J???7777!7??JJ55YY??JJJ??JJ
&@&&@&&&P55YYYYJJJJJJJJJJJJJJJJYBPPGGPPPGGGGPPGPP55YJ??7??????????JJYYY55Y??7777777?JY555YJJJJYJ??JJ
&@&&&@@BP5YYYYYJJJJJ??JJJ???JJJJGP55PP5Y5P5YJJ?J?77777777777?????JJJJYY55YJ?777!!77JJ555PYYJJYYJ?JJ?
&@@&&@&BPYYYYJJJJJJJ?JJJ????JJ?YBPY5P5Y??7777!!77777777777777????JJJJJY55PJ7??777!???JY55YYJYYYJJJJ?
&&&&@@&G5YYYJJJJJJJJJJJJ??JJJJJYGPY555YJ?77777777777777777777????JJJJJYY5P5?7??7!7??77?Y55YY55YJ????
&&&&&&#BGGGPPPPPPPP5555555555YYY5P5Y555Y?777777777777777777777???JJJJJYY5Y7!!7????JJ???J555YP5J????J
YYJYY555YYJYY5YJJYYYYJJJJJJJ?7!!J55YY55YJ??7777777777777777777????J?JJJJ?!~~~~!?JY??7???JYY55YJJJJJJ
????JJYYJ????7777!!7??JJJJJJJJJ?JYYYY55Y???77777777777777777777????JJ?!~~~~~~~~!??J?77!!7?J?JYYYYYYY
???7!!!!7??JJJJJJ?77!!!!!!7????7?YYYYYYJ???777777777777777777777???7!~~~~~~~~~~!!7?JJ??JJJJJ?7777777
?7!~~~~~~~~~!!7?JJJ????7!!?YYY?7J55YYYYJ??77777777777777777777777!~~~~~~~~~~~!!777777?JJJJ??7!!!!!!!
??7!!~~~~~~!!~~~!!7??JJJ?777JJ775P5YYYYJJ?77777777777777777777!!~^^~~~^~~~~!777?JYJ7!!!777777777!!7?
JJ???77!!!!~~!~~~~~~!7?77!!7??7JP5PYJJJJJ??77777777777!!777!~~~~~^^~~~~~~!!77!~!7JYJ?777!7????JJ????
~!7?JJJJJJ?77!~~~!7???????7??7JPY5PYJJJ????7777777777777!~~~~~~~~~~~~~!7?J7!!!!!77?JJJ?JJ?77777?JJJJ
~~~~~!777JJJ??777?JJJJJJJJY?7?55JY5YJ??????77777???777!~~~~~~~~~~!!!!!7JJJJJJJJ?????7777???77??77!!!
~~~~!~~~~~~7??JJ??77!!!??JJ7?Y5YYY5Y???777?77?????7!~^~~~~~~~~!!!!77!~~~!77???JYYJJJJ??????7!~~~~~!!
!!!!~~~~^~~~~~!777!!77!???77J5YYYY5Y?77777?????7!~~~~~~~~~~~~~!!!!~^~~~~~~~!~~~!!7?JJJJ?!~~~^~~^^!77
?????7!~~~~~~!!!777777?JJ77J55YY5Y5YJ7777??7!~~~~~~~~~~~~~~~~!7??7!!~~~~~~~~~~~~~~~~~!!!~~~~~~~~!7JJ
7?JJJ???777777??JJJJJJ5Y77?Y5555555Y?77!!!~~~~~~~~~~~~!!77!!7?JJ???????7777!!!~~~~!!~!7??7?77777?7!!
~~~!7?JJJJJJJJJJJ??????!??JJJ???7!!~~~~~~~~~~~~~~~~!7??7??7!~~~~!7????JJJJJ?????777777?JJJJJJJ???!~!
~~~~~!!777777!!!!!!7777!!!~~~~~~~~~~~~~~~~~~~~~~~~~~~!!!7!~~^~^^~^~~~~~~!!7?JJJJJJJJJ?7!!!!!!~~!!!!7
*/