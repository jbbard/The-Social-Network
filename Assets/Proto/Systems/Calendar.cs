using Unity.Entities;

namespace Dummy.Systems
{
    public struct Daytime : IComponentData
    {
        public int day;
        public int hour;
        public int minute;
        public float elapsedTime;
    }

    public struct TimeSpeed : IComponentData
    {
        public float value;
    }

    // TODO convert to system group
    public class Calendar : SystemBase
    {
        protected override void OnCreate()
        {
            // TODO: Remove when save
            var time = EntityManager.CreateEntity(typeof(Daytime), typeof(TimeSpeed));
            EntityManager.SetComponentData(time, new TimeSpeed {value = 1f});
            EntityManager.SetName(time, "Daytime");
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((
                ref Daytime daytime,
                in TimeSpeed timeSpeed) =>
            {
                daytime.elapsedTime += deltaTime * timeSpeed.value;

                if (daytime.elapsedTime >= 1f)
                {
                    daytime.elapsedTime -= 1f;

                    if (daytime.minute == 59)
                    {
                        daytime.minute = 0;

                        if (daytime.hour == 23)
                        {
                            daytime.hour = 0;
                            ++daytime.day;
                        }
                        else
                        {
                            ++daytime.hour;
                        }
                    }
                    else
                    {
                        ++daytime.minute;
                    }
                }

            }).Schedule();
        }
    }
}