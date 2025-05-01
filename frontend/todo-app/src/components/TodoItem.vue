<template>
  <div class="todo-item" :class="{ 'completed': todoData.isCompleted }">
    <div v-if="!editing" class="todo-content">
      <div class="todo-info">
        <h3 class="todo-title">{{ todoData.title }}</h3>
        <p class="todo-description">{{ todoData.description }}</p>
      </div>
      
      <div class="todo-actions">
        <label class="checkbox-container">
          <input 
            type="checkbox" 
            v-model="todoData.isCompleted"
            @change="updateTodo"
          />
          <span class="checkmark"></span>
        </label>
        <button @click="startEditing" class="btn-edit">Edit</button>
        <button @click="deleteTodo" class="btn-delete">Delete</button>
      </div>
    </div>
    
    <div v-else class="todo-edit-form">
      <div class="form-group">
        <label for="edit-title">Title</label>
        <input 
          type="text" 
          id="edit-title" 
          v-model="todoData.title" 
          required
        />
      </div>
      
      <div class="form-group">
        <label for="edit-description">Description</label>
        <textarea 
          id="edit-description" 
          v-model="todoData.description"
        ></textarea>
      </div>
      
      <div class="edit-actions">
        <button @click="saveEdit" class="btn-save">Save</button>
        <button @click="cancelEdit" class="btn-cancel">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TodoItem',
  props: {
    todo: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      todoData: { ...this.todo },
      editing: false,
      originalTodo: null
    }
  },
  methods: {
    updateTodo() {
      this.$emit('todo-updated', { ...this.todoData })
    },
    
    deleteTodo() {
      if (confirm('Are you sure you want to delete this todo?')) {
        this.$emit('todo-deleted', this.todoData.id)
      }
    },
    
    startEditing() {
      this.originalTodo = { ...this.todoData }
      this.editing = true
    },
    
    saveEdit() {
      this.editing = false
      this.updateTodo()
    },
    
    cancelEdit() {
      this.todoData = { ...this.originalTodo }
      this.editing = false
    }
  }
}
</script>

<style scoped>
.todo-item {
  background-color: var(--card-background);
  padding: 15px;
  border-radius: 8px;
  box-shadow: var(--shadow);
  margin-bottom: 15px;
  transition: all 0.3s ease;
}

.todo-item.completed {
  background-color: rgba(76, 175, 80, 0.1);
}

.todo-content {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.todo-info {
  flex-grow: 1;
}

.todo-title {
  margin-top: 0;
  margin-bottom: 5px;
  color: var(--text-color);
  transition: color 0.3s ease;
}

.todo-description {
  color: var(--text-color);
  opacity: 0.7;
  margin: 0;
  transition: color 0.3s ease;
}

.todo-actions {
  display: flex;
  align-items: center;
  gap: 10px;
}

.checkbox-container {
  position: relative;
  display: inline-block;
  width: 20px;
  height: 20px;
  cursor: pointer;
}

.checkbox-container input {
  opacity: 0;
  width: 0;
  height: 0;
}

.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 20px;
  width: 20px;
  background-color: var(--card-background);
  border: 2px solid var(--primary-color);
  border-radius: 3px;
  transition: background-color 0.3s ease;
}

.checkbox-container:hover input ~ .checkmark {
  background-color: rgba(76, 175, 80, 0.1);
}

.checkbox-container input:checked ~ .checkmark {
  background-color: var(--primary-color);
}

.checkmark:after {
  content: "";
  position: absolute;
  display: none;
}

.checkbox-container input:checked ~ .checkmark:after {
  display: block;
}

.checkbox-container .checkmark:after {
  left: 6px;
  top: 2px;
  width: 5px;
  height: 10px;
  border: solid white;
  border-width: 0 2px 2px 0;
  transform: rotate(45deg);
}

.btn-edit, .btn-delete, .btn-save, .btn-cancel {
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  transition: all 0.3s ease;
}

.btn-edit {
  background-color: var(--secondary-color);
  color: white;
}

.btn-delete {
  background-color: var(--delete-color);
  color: white;
}

.btn-save {
  background-color: var(--primary-color);
  color: white;
}

.btn-cancel {
  background-color: var(--card-background);
  color: var(--text-color);
  border: 1px solid var(--border-color);
}

.todo-edit-form {
  margin-top: 10px;
}

.edit-actions {
  margin-top: 10px;
  display: flex;
  gap: 10px;
}

.completed .todo-title {
  text-decoration: line-through;
  color: var(--text-color);
  opacity: 0.7;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: var(--text-color);
}

input, textarea {
  width: 100%;
  padding: 8px;
  border: 1px solid var(--border-color);
  border-radius: 4px;
  font-size: 14px;
  background-color: var(--card-background);
  color: var(--text-color);
}

textarea {
  height: 80px;
  resize: vertical;
}
</style>