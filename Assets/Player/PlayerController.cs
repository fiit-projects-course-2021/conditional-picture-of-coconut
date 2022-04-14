using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5f;
	public Rigidbody2D rb;
	public Weapon weapon;
    private Grid grid;

	private PlayerInput playerInput;
	private Vector2 cursorPosition;
	private Vector2 moveDirection;
    private int2 gridPosition;
	
    private void Start()
    {
        grid = new Grid(5, 5, 1);
        gridPosition = grid.GetGridPosition(transform.position);
    }

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
	}
	private void Update()
	{
		moveDirection = playerInput.MovementInput.normalized;
		cursorPosition = Camera.main.ScreenToWorldPoint(playerInput.AimingInput);
		CheckForFire();
	}

    private void FixedUpdate()
	{
		Move();
		Aim();
        UpdateGridPosition();
    }

	private void Move()
	{
		rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
	}
	
	private void Aim()
	{
		var aimDirection = cursorPosition - rb.position;
		var aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
		rb.rotation = aimAngle;
	}

    private void UpdateGridPosition()
    {
        var newGridPosition = grid.GetGridPosition(transform.position);
        if (newGridPosition.x != gridPosition.x || newGridPosition.y != gridPosition.y)
        {
			grid.UnFillCell(new Vector3(gridPosition.x, gridPosition.y));
			grid.FillCell(new Vector3(newGridPosition.x, newGridPosition.y));
		}

        gridPosition = newGridPosition;
    }

    private void CheckForFire()
	{
		if (playerInput.IsFireInput)
			weapon.Fire();
	}
}