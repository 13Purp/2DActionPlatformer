//Work in progress//

A 2D action platformer scripted in C#, unity being used mainly for rendering and collision detection

Currently implemented:

  -Controller interface
	
    	-Currently used by the PlayerController and Enemy classes
    -Classes implementing the interface deal with moving their respective gameObject and performing certain actions
    -Classes currently inheriting the Enemy class
        -BlobController, an enemy type similar to Super Mario's goombas with added patrolling behaviours and player detection
        -AnxElenaController, an enemy type featuring the same patrolling behaviours and player detection, but holding a weapon that fires WindBullets (for the purpose of pushing the player around) 
    
  -Bullet interface
	
    -Declares Shoot and strongShoot methods used for different modes of firing
    -Currently used by WindBullet and ZapBullet classes
        -WindBullets are Physics objects used for the purpose of pushing other Physics objects
            -Calling their Shoot method acts as a rapid fire, while the strongShoot method acts more like a shotgun
        -ZapBullets represent a bolt of electricty with randomly generated visuals used for the purpose of damaging Enemies
            -Calling their Shoot method, again, acts as a rapid fire, while the strongShoot method acts as a bolt of thunder exploding on impact
    
  -Weapon class
	
    -The weapon is held by objects implementing the Controller interface and "fires"  Bullets
    -Weapons follow a target (The mouse cursor in case of Player holding it or a position set by the object holding the weapon)
    -The switching of bullets is implemented using the Strategy design pattern
  
  -WorldCache class
	
    -Implemented as a Thread-safe Singleton
    -Used to store Physics objects and Controllers in a dictionary, for the purpose of speeding up interactions
    
  -MovingPlatform class
	
    -Platforms that are physics objects and thus, react to gravity and other Physics objects (such as the Player, Enemies, WindBullets, etc...)
    -While traveling at sufficiently high speeds, are capable of "hurting" Enemies on collision by calling their TakeDamage method

