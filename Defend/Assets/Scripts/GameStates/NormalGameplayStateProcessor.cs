namespace GameStates {
    public class NormalGameplayStateProcessor : GameplayStateProcessor {
        public NormalGameplayStateProcessor(GameplayStateContext ctx) : base(ctx) { }
        
        public override void OnEnterState() { 
            base.OnEnterState();
            Ctx.GridHighlighter.gameObject.SetActive(false);
        }

        public override void HandleKeyboardInput() {
            // if this remains unused consider composition over inheritance
        }

        public override void Update() {
            // if this remains unused consider composition over inheritance
        }
    }
}