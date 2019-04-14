local player;
local yellowHair;
local redGlass;
local twinTail;

local playerWalkCoroutine;

function awake()
    player = scene.entity("Player");
    yellowHair = scene.entity("YellowHair");
    redGlass = scene.entity("RedGlass");
    twinTail = scene.entity("TwinTail");

    yellowHair.on("OnInteract", function()
        --game.control(null)
        gui.skillPanel.show()
    end)
    --playerWalkCoroutine = startCoroutine(playerWalk);
    camera.reset();
    camera.follow(player);
    game.control(player);
    gui.skillPanel.hide();
end
function start()
    --startCoroutine(moveCamera); 
end

function moveCamera()
    coroutine.yield(waitForSeconds(1.5));
    stopCoroutine(playerWalkCoroutine);
    coroutine.yield(waitForSeconds(1));
    camera.speed = 3;
    camera.acceleration = 3;
    camera.follow(twinTail);
    coroutine.yield(waitForSeconds(.5));
    camera.follow(yellowHair);
    coroutine.yield(waitForSeconds(4));
    camera.follow(player);
    coroutine.yield(waitForSeconds(3));
    game.control(player);
    game.loadScene("Scenes/Credit");
end

function playerWalk()
    while true do
        player.move(vec2(.3, 0));
        coroutine.yield(nil);
    end
end
