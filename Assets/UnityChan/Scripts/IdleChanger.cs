using UnityEngine;
using System.Collections;
using Leap;

[RequireComponent(typeof(Animator))]

public class IdleChanger : MonoBehaviour
{
	
	private Animator anim;

    private AudioSource[] music;

    private AnimatorStateInfo currentState;
    private AnimatorStateInfo previousState;

    private Analyzer1 analyzer1;
    private Analyzer2 analyzer2;
    private Analyzer3 analyzer3;
    private bool start;
	

	void Start ()
	{
		anim = GetComponent<Animator> ();

        music = GetComponents<AudioSource>();
  
		analyzer1 = new Analyzer1();
        analyzer2 = new Analyzer2();
        analyzer3 = new Analyzer3();

        //anim.SetBool("Stop", false);
        anim.SetInteger("Next", -1);

        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;
        start = false;
	}

    void OnGUI()
    {
        Controller controller = new Controller();
        Frame frame = controller.Frame();
        GUI.Box(new Rect(Screen.width - 110, 10, 100, 90), "Start Game");
        GUI.Button(new Rect(Screen.width - 100, 40, 80, 20), "Start");
        if (start = false && (analyzer1.Analyze(frame) || Input.GetKeyDown(KeyCode.A)))
            start = true;
//         if (GUI.Button(new Rect(Screen.width - 100, 70, 80, 20), "Quit") || analyzer2.Analyze(frame))
//             anim.SetBool("Back", true);
    }

    void Update()
    {
        
            Controller controller = new Controller();
            Frame frame = controller.Frame();
            if (frame == null)
                return;

            else if (anim.GetInteger("Next") == -1)
            {
                if (Input.GetKeyDown(KeyCode.W) || analyzer1.Analyze(frame))
                {
                    music[0].Play();
                    anim.SetInteger("Next", 0);
                }
                else if (Input.GetKeyDown(KeyCode.Q) || analyzer3.Analyze(frame))
                {
                    anim.Play("WALK00_F");
                    music[0].Play();
                }
                else
                {
                    currentState = anim.GetCurrentAnimatorStateInfo(0);
                    if (previousState.nameHash != currentState.nameHash)
                    {
                        //anim.SetBool("Stop", true);
                        anim.SetInteger("Next", -1);
                        previousState = currentState;
                    }
                }

            }

            else if (anim.GetInteger("Next") == 0)
            {
                if (Input.GetKeyDown(KeyCode.S) || analyzer2.Analyze(frame))
                {
                    music[0].Play();
                    anim.SetInteger("Next", 1);
                }
                else
                {
                    currentState = anim.GetCurrentAnimatorStateInfo(0);
                    if (previousState.nameHash != currentState.nameHash)
                    {
                        //anim.SetBool("Stop", true);
                        anim.SetInteger("Next", -1);
                        previousState = currentState;
                    }
                }
            }

            else if (anim.GetInteger("Next") == 1)
            {
                if (Input.GetKeyDown(KeyCode.X) || analyzer2.Analyze(frame))
                {
                    music[0].Play();
                    anim.SetInteger("Next", 2);
                }
                else
                {
                    currentState = anim.GetCurrentAnimatorStateInfo(0);
                    if (previousState.nameHash != currentState.nameHash)
                    {
                        //anim.SetBool("Stop", true);
                        anim.SetInteger("Next", -1);
                        previousState = currentState;
                    }
                }
            }
        }
        

    

}
