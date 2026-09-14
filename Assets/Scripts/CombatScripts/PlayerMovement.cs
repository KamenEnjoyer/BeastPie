using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Transform contentParent;

    [Header("Movement")]
    public float moveSpeed = 4.5f;
    public float sprintMultiplier = 1.8f;

    [Header("Stamina")]
    public Image stamina;
    public float staminaDrainRate = 0.1f;
    public float staminaRegenRate = 0.2f;
    public float staminaRegenDelay = 1f;
    private float staminaRegenTimer = 0f;

    [Header("MelleAttack")]
    public GameObject lightSlashPref;
    public GameObject heavySlashPref;
    public float slashDistance = 0.6f;
    public float slashLifetime = 0.15f;
    public float heavyAttackLockTime = 0.5f;
    public float lightAttackDashDistance = 0.25f;
    public float lightAttackDashTime = 0.05f;

    [Header("RangeAttack")]
    public GameObject tensionForcePref;
    public GameObject arrowPref;
    public float rangeAttackDuration = 3f;
    public float deceleration = 0.5f;

    private bool melleAttacking = true;
    private PlayerTension tentionForce = null;

    private Vector2 dashVelocity;
    private float dashTimer;
    private bool canAttack = true;
    private bool movementLocked = false;
    private bool isAiming = false;
    private bool isReloaded = false;

    private Rigidbody2D rb;
    private Rigidbody2D rbTensionBar;
    private Vector2 movement;

    private Camera mainCamera;
    private Vector2 screenMin;
    private Vector2 screenMax;
    private Vector2 halfSize;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        screenMin = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        halfSize = GetComponent<SpriteRenderer>().bounds.extents;
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 1f) movement = movement.normalized;
        if (Input.GetMouseButtonDown(0))
        {
            if (melleAttacking) MelleAttack(true, lightSlashPref);
            else RangedAttack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (melleAttacking) MelleAttack(false, heavySlashPref);
            else RangedAttackReloading();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            melleAttacking = !melleAttacking;
        }

        RotateTowardsMouse();
    }

    public void ArrowSpawn(float damage)
    {
        isAiming = false;

        Vector2 shootDirection = transform.up;
        Vector2 spawnPosition = (Vector2)transform.position + shootDirection * 0.6f;

        GameObject arrow = Instantiate(arrowPref, spawnPosition, Quaternion.identity);

        ProjectileSpawn projectile = arrow.GetComponent<ProjectileSpawn>();

        projectile.Init(shootDirection, damage, false, 10f);
    }

    private void RangedAttack()
    {
        if (!canAttack || !isReloaded) return;
        if (stamina.fillAmount < 0.1f) return;

        canAttack = false;
        isAiming = true;
        StartCoroutine(AttackCooldown(1f));

        Vector2 attackDirection = transform.up;
        Vector2 spawnOffset = Vector2.zero;

        stamina.fillAmount -= 0.1f;
        spawnOffset = attackDirection;

        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        tentionForce = Instantiate(tensionForcePref, contentParent).GetComponent<PlayerTension>();
        rbTensionBar = tentionForce.gameObject.GetComponent<Rigidbody2D>();

        if (isReloaded) isReloaded = false;
    }

    private void RangedAttackReloading()
    {
        if(!isReloaded) isReloaded = true;
    }

    private void MelleAttack(bool light, GameObject slashPref)
    {
        if (!canAttack) return;
        if ((light && stamina.fillAmount < 0.07f) || (!light && stamina.fillAmount < 0.2f)) return;

        canAttack = false;
        StartCoroutine(AttackCooldown(0.25f));

        Vector2 attackDirection = transform.up;
        Vector2 spawnOffset = Vector2.zero;
        if (light)
        {
            stamina.fillAmount -= 0.07f;
            spawnOffset = attackDirection * slashDistance;
        }
        else stamina.fillAmount -= 0.2f;

        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
        GameObject slash = Instantiate(slashPref, spawnPosition, Quaternion.identity);

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        slash.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        Destroy(slash, slashLifetime);

        if (light) 
        {
            dashVelocity = attackDirection.normalized * (lightAttackDashDistance / lightAttackDashTime);
            dashTimer = lightAttackDashTime;
        }
        else StartCoroutine(HeavyAttackLock());
    }

    private IEnumerator HeavyAttackLock()
    {
        movementLocked = true;
        canAttack = false;
        yield return new WaitForSeconds(heavyAttackLockTime);
        movementLocked = false;
        canAttack = true;
    }

    private IEnumerator AttackCooldown(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        if (movementLocked) return;

        float currentSpeed = moveSpeed;
        bool isMoving = movement.sqrMagnitude > 0.01f;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina.fillAmount > 0f && isMoving;

        if (isSprinting || isAiming)
        {
            if (isAiming)
            {
                isSprinting = false;
                currentSpeed *= deceleration;
            }
            else
            {
                currentSpeed *= sprintMultiplier;
                stamina.fillAmount -= staminaDrainRate * Time.fixedDeltaTime;
                stamina.fillAmount = Mathf.Max(stamina.fillAmount, 0f);
            }
            staminaRegenTimer = staminaRegenDelay;
        }
        else if (staminaRegenTimer > staminaDrainRate) staminaRegenTimer -= staminaDrainRate;
        else
        {
            stamina.fillAmount += staminaRegenRate * Time.fixedDeltaTime;
            stamina.fillAmount = Mathf.Min(stamina.fillAmount, 1f);
        }

        Vector2 finalMovement = movement * currentSpeed;
        if (dashTimer > 0f)
        {
            finalMovement += dashVelocity;
            dashTimer -= Time.fixedDeltaTime;
        }
        Vector2 targetPosition = rb.position + finalMovement * Time.fixedDeltaTime;
        targetPosition.x = Mathf.Clamp(targetPosition.x, screenMin.x + halfSize.x, screenMax.x - halfSize.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, screenMin.y + halfSize.y, screenMax.y - halfSize.y);

        if (tentionForce) { rbTensionBar.MovePosition(targetPosition); }
        
        rb.MovePosition(targetPosition);
    }

    private void RotateTowardsMouse() //REFACTOR
    {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if (tentionForce)
        {
            tentionForce.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            tentionForce.transform.position = rb.position + new Vector2(Mathf.Cos(Mathf.PI * angle / 180f), Mathf.Sin(Mathf.PI * angle / 180f));
        }
    }
}
