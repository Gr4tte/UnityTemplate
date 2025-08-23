# Unity C# Coding & Project Style Guide
Latest version can be found here - https://github.com/Gr4tte/Unity-Style-Guide
## File Structure
`!Project` - Contains all user created files aside from "Resources" folder
- `Art` - any graphics asset (`sprites`, `animations`, `textures`)
- `Audio`
- `Code`
  - `Scripts`
    - `Debug` - Visalizers and game modifying scripts meant for bug finding and testing
    - `Managers` - Singletons for one scene
    - `Systems` - Persistant Singletons across all scenes
    - `Utils` - static helpers
    - `UI` - Interactable interfaces
    - `UX` - Effects and feedback
    - `Editor` - Editor windows
  - `Shaders`

- `Design` - All non graphics, audio or code assets
  - `Scenes`
    - `Dev` - developing but not currently in the game/build
    - `Prod` - included in the build and used in the game
    - `Sandbox` - test and personal scenes to play around in
  - `Prefabs`
  - `Scriptable Objects`
- `Documentation`

## Naming Conventions

### General
- **No abbreviations** unless industry-standard (`AI`, `UI`).
- **Descriptive but concise**. Don’t repeat class name in variable names.
  ```csharp
  // ❌ Avoid
  [SerializeField] private float _characterSpeed;
  
  // ✅ Do
  [SerializeField] private float _speed;
### Classes
  - `PascalCase`
  - `ScriptableObjects` end with `SO`
    ```csharp
    public class EnemySO : ScriptableObject { }
  - **Singletons**
    - **Normal** ends with `Manager`
    - **Persistant** ends with `System`
    ```csharp
    public class EntityManager : Singleton<EntityManager> { }
    public class AudioSystem : PersistantSingleton<AudioSystem > { }
### Interfaces
  - `PascalCase`
  - Prefixed by `I`
	  ```csharp
	  public interface IWeapon { }
### Methods
  - `PascalCase`
  - Start with a verb
	  ```csharp
	  private void DoSomething() { }
	public int GetHealth() { return _health; }
### Fields, Properties and Variables
  - **Private**
    - `camelCase` 
    - prefixed by `_`
    ```csharp
    private int _health;
  - **Exposed**
    - `camelCase` 
    - prefixed by `_`
    - using `SerializeField` attribute
    ```csharp
    [SerializeField] private float _speed;
  - **Public fields/properties**
    - `PascalCase` 
    - Using `field: SerializeField` if inspector editable
    - Avoid public setters
    ```csharp
    public int Health { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
  - **Scoped variables**
    - `camelCase` 
    ```csharp
    int localCounter = 0;
  - **Constants**
    - `UPPER_CASE` 
    - use a serialized field instead when possible
    ```csharp
	private const int MAX_ENTITIES = 100;
### Booleans
  - prefix with (`is`, `has`, `can`, etc)
  - keep positive
	  ```csharp
	  // ❌ Avoid  
	 public bool Grounded { get; private set; }
	 private bool _isDead
	  
	  // ✅ Do  
	 public bool IsGrounded { get; private set; }
	 private bool _isAlive;
### Collections
  - always plural
	  ```csharp
	private List<int> _numbers;
	private string[] _names;
## Unity Specific
  - `Awake` - cache reference (don't call another class' methods or fields)
  - `Start` - initialization requiring another class' methods and fields
  - `Update` - gameplay loop 
    - Avoid `FindObjectOfType` or `GetComponent`
    - Avoid code other than method calls
## Patterns & Anti-Patterns
✅ Do
  - Use `ScriptableObject` for shared shared or large datasets (enemies, player, items)
  
❌ Don’t
   - Don't use `public` fields to expose members (use `[SerializeField] private`)
   - Don't define more than one class per file
## Code Style
### Formatting
  - **Indentation style** - Tabs
  - **Tab size** - 4
  - **Braces** - New line
	  ```csharp
	  if (_isAlive)
	  {
	      DoSomething();
	  }
### Construction & Typing
  - Use explicit types unless it's in a loop
    ```csharp
    // ❌ Don't
    var number = 2f;
    var isAlive = true;

	// ✅ Do
	float number = 2f;
	bool isAlive = true;
	foreach(var element in elements)
	{
		DoSomething();
	}
    ```
  - Use `new` instead of explicit constructors
	  ```csharp
	  // ❌ Don't
	  Vector3 position = new Vector3(0f, 1f, 0f);

	  // ✅ Do
	  Vector3 position = new(0f, 1f, 0f);
### Order in Classes
  1. Constants
  2. Public exposed fields
  3. Private exposed fields
  4. Public fields
  5. Private fields
  6. `Awake`
  7. `OnEnable`
  8. `OnDisable`
  9. `Update`
  10. Public methods
  11. Private methods
  12. Nested structs/Enums
### Nesting
Avoid deep nesting by early returns
```csharp
// ❌ Don’t
if (_isAlive)
{
    DoStuff();
}

// ✅ Do
if (!_isAlive) return;
DoStuff();
```
### Ternary Operators
Use ternary Operators when it improves readability
```csharp
// ❌ Don’t
if (_isGrounded)
{
    _velocity.y = 0;
}
else
{
    _velocity.y = _newVelocity.y;
}

// ✅ Do
_velocity.y = _isGrounded ? 0 : _newVelocity.y;
```
### Line Breaks
Break long lines of code inte several lines
```csharp
// ❌ Don’t
_myFloat = (someCalulation) * (anotherCalculation) - yetAnotherCalculation;
transform.position = new Vector3(someCalculation, anotherCalculation, yetAnotherCalculation);
transform.DoMoveX(1f, 1f).SetEase(Ease.Linear).SetLooping(-1).SetInverted().OnComplete(MyFunc);
	
// ✅ Do
float val1 = someCalulation;
float val2 = anotherCalculation;
float val3 = yetAnotherCalculation;
_myFloat = val1 * val2 - val3;

transform.position = new Vector3(
    someCalculation,
    anotherCalculation,
    yetAnotherCalculation);

transform.DoMoveX(1f, 1f)
	.SetEase(Ease.Linear)
	.SetLooping(-1)
	.SetInverted()
	.OnComplete(MyFunc);
```
## Example Class
```csharp
using UnityEngine;

namespace ProjectName
{
	public class ClassName
	{
		private const int MAX_ENTITIES;
		[field: SerializeField] public float MaxHealth { get; private set; }
		[SerializeField] private float _speed ;

		public float Health { get; private set; }
		private SpriteRenderer _renderer;
		private PlayerInputActions _inputActions;
		private List<Enemy> _enemies;
		
		private void Awake()
		{
			_renderer = GetComponent<SpriteRenderer>();
			_inputActions = new();

			Health = MaxHealth;
		}

		private void OnEnable()
		{
			_inputActions.Enable();
		}

		private void OnDisable()
		{
			_inputActions.Disable();
		}

		private void Start()
		{
			_enemies = EntityManager.Instance.GetEnemies();
		}

		private void Update()
		{
			Move();
		}

		public void TakeDamage(float damage)
		{
			Health -= damage;
		}

		private void Move()
		{
			//Do movement calculations
		}

		private struct ExampleStruct()
		{
			public int Number;
			public bool Boolean;
		}
	}
}
```
