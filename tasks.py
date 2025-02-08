# Менеджер задач
task_manager = {}

def add_task(name, deadline):
    """Добавляет новую задачу."""
    if name not in task_manager:
        task_manager[name] = {'Статус': 'В процессе', 'Дедлайн': deadline}
        print(f"Задача '{name}' успешно добавлена.")
    else:
        print(f"Задача '{name}' уже существует.")

def get_task(name):
    """Получает информацию о задаче."""
    if name in task_manager:
        print(f"Задача: {name}")
        print(f"Статус: {task_manager[name]['Статус']}")
        print(f"Дедлайн: {task_manager[name]['Дедлайн']}")
    else:
        print(f"Задача '{name}' не найдена.")

def update_status(name, status):
    """Обновляет статус задачи."""
    if name in task_manager:
        task_manager[name]['Статус'] = status
        print(f"Статус задачи '{name}' обновлен на '{status}'.")
    else:
        print(f"Задача '{name}' не найдена.")

def delete_task(name):
    """Удаляет завершенную задачу."""
    if name in task_manager:
        if task_manager[name]['Статус'] == 'Завершена':
            del task_manager[name]
            print(f"Задача '{name}' удалена.")
        else:
            print(f"Задача '{name}' не может быть удалена, так как её статус не 'Завершена'.")
    else:
        print(f"Задача '{name}' не найдена.")

# Пример использования
add_task("Подготовить презентацию", "2023-12-01")
get_task("Подготовить презентацию")
update_status("Подготовить презентацию", "Завершена")
delete_task("Подготовить презентацию")
