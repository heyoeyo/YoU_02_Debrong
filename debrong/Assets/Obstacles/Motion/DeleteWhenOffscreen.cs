using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWhenOffscreen : MonoBehaviour {

    [SerializeField, Range(0.5f, 5f)] private float seconds_per_check = 3f;
    [SerializeField] private Vector2 out_of_bounds_distance = new Vector2(20f, 15f);


    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    void Start() {
        // For efficiency, only check for deletions 'occasionally'
        StartCoroutine(DeleteOutOfBounds());
    }

    IEnumerator DeleteOutOfBounds() {

        while (true) {
            yield return new WaitForSeconds(this.seconds_per_check);

            bool is_offscreen_x = Mathf.Abs(this.transform.position.x) > out_of_bounds_distance.x;
            bool is_offscreen_y = Mathf.Abs(this.transform.position.y) > out_of_bounds_distance.y;
            if (is_offscreen_x || is_offscreen_y) {
                Destroy(this.gameObject);
            }

        }

    }


    // ----------------------------------------------------------------------------------------------------------------
    // Debugging

    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;

        Vector3 gizmo_size = out_of_bounds_distance;
        Gizmos.DrawWireCube(Vector3.zero, 2 * gizmo_size);
    }
}
