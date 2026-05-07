Анти-патерн "God Object" 

1. God Object — це об'єкт, який робить занадто багато. Це класичний приклад порушення принципів проектування, коли один клас бере на себе занадто велику кількість обов’язків, стаючи центральним вузлом усієї логіки програми.

Основні характеристики:
1.Величезний розмір: Клас містить сотні або тисячі рядків коду.
2.Низька згуртованість (Low Cohesion): Методи всередині класу майже не пов'язані між собою логічно. Наприклад, один метод працює з базою даних, інший — з інтерфейсом користувача, а третій — з розрахунком податків.
3.Висока зв’язність (High Coupling): Оскільки цей клас керує всім, майже всі інші частини системи залежать від нього. Зміна в одному місці ламає все.
4.Складність тестування: Написати unit-тести для такого класу майже неможливо, бо він тягне за собою всю інфраструктуру додатка.

2. Приклад простого класу, який порушує SRP

class OrderManager:
    def __init__(self, order_data):
        self.order_data = order_data

    def calculate_total(self):
        # Логіка розрахунку ціни
        return sum(item['price'] for item in self.order_data['items'])

    def save_to_database(self):
        # Логіка підключення до БД та збереження
        print(f"Збереження замовлення {self.order_data['id']} у базу даних...")

    def send_confirmation_email(self):
        # Логіка відправки листа користувачу
        print(f"Відправка листа на {self.order_data['customer_email']}...")

Чому клас OrderManager порушує SRP?
Принцип єдиної відповідальності (Single Responsibility Principle) стверджує: "Клас повинен мати лише одну причину для зміни". У нашому прикладі причин для зміни мінімум три:

Бізнес-логіка: Змінився алгоритм розрахунку податків або знижок (calculate_total).

Інфраструктура БД: Ми вирішили перейти з SQL на MongoDB (save_to_database).

Комунікації: Ми вирішили надсилати SMS замість Email або змінили сервіс розсилки (send_confirmation_email).

3. Рефакторинг
Щоб виправити це, ми повинні розділити обов'язки між різними класами. Кожен клас повинен відповідати за свою вузьку область.

1.Модель даних та розрахунки
class Order:
    def __init__(self, order_id, items, customer_email):
        self.order_id = order_id
        self.items = items
        self.customer_email = customer_email

    def get_total_price(self):
        return sum(item['price'] for item in self.items)

2.Робота з базою даних (Repository)
class OrderRepository:
    def save(self, order):
        # Тільки код для роботи з БД
        print(f"Об'єкт {order.order_id} збережено.")

3.Сервіс сповіщень
class NotificationService:
    def send_email(self, email, message):
        # Тільки код для відправки пошти
        print(f"Лист відправлено на {email}: {message}")