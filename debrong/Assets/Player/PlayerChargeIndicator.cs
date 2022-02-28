using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeIndicator : MonoBehaviour
{
    SpriteRenderer charge_sprite;
    float player_height;

    void Awake() {
        this.charge_sprite = this.GetComponent<SpriteRenderer>();
    }

    void Start() {

        BoxBounds player_bounds = this.GetComponentInParent<PlayerShape>().GetShapeBounds();

        SetInitialX(player_bounds);

        // Store player height for scaling charge ui
        player_height = player_bounds.height;
    }

    private void OnEnable() {
        PlayerChargingEventManager.SubscribeChargingEvents(UpdateChargingAppearance);
    }

    private void OnDisable() {
        PlayerChargingEventManager.UnsubscribeChargingEvents(UpdateChargingAppearance);
    }

    void SetInitialX(BoxBounds player_bounds) {

        // Function makes sure the charging sprite is located 'behind' the player shape

        // Figure out positioning, by placing the sprite to the left of the player left-most edge
        float shape_left = player_bounds.left_x;
        float half_width = this.transform.localScale.x / 2f;
        float x_offset = shape_left - (half_width + 0.1f);

        // Update only the x positioning
        Vector3 sprite_pos = this.transform.position;
        Transform player_transform = this.transform.parent;
        sprite_pos.x = player_transform.position.x + x_offset;
        this.transform.position = sprite_pos;
    }

    void UpdateChargingAppearance(float charge_amount) {
        Vector3 curr_localscale = this.transform.localScale;
        curr_localscale.y = player_height * charge_amount;
        this.transform.localScale = curr_localscale;
    }

}
