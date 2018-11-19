﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody m_rb;
    public float speed = 10.0F ;
    public float max_speed = 15.0F ;
    public float jump_height = 450F ;
    private Collider m_collider;
    private float collider_radius = 0.0F;
    private float grounded_epsilon = 0.05F;
    public int user_layer_platform;
    private float get_axis_horizontal = 0.0f;
    private bool get_key_down_space = false;
    public string pickup_tag;
    public GameManager gm;
    public string livebox_tag;
    public string finish_tag;
    // Use this for initialization
    void Start () {
        m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        collider_radius = m_collider.bounds.extents.y;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        /* get user input to apply a horizontal
         * force to our player object */

        float movement = Input.GetAxis("Horizontal");
        //add force
        m_rb.AddForce(new Vector3(movement * speed, 0.0F, 0.0F));
        m_rb.velocity = new Vector3(Mathf.Clamp(m_rb.velocity.x,
            -max_speed, max_speed),m_rb.velocity.y,m_rb.velocity.z);

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            m_rb.AddForce(0.0F, jump_height, 0.0F);
        }
        
    }
    bool isGrounded()
    {
        int platform_layer = 1 << user_layer_platform;
        return Physics.Raycast(transform.position,
            Vector3.down, collider_radius + grounded_epsilon,
            platform_layer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(pickup_tag)){
            other.gameObject.gameObject.SetActive(false);
            gm.score += 25; 
        }
        if (other.gameObject.CompareTag(finish_tag))
        {
            gm.score += 500;
            gm.level_finished();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(livebox_tag))
        {
            gm.game_over();
        }
    }
}
